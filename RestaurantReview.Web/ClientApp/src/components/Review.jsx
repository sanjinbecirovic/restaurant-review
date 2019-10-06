import React from "react";
import moment from "moment";
import { inject } from "mobx-react";
import { Comment, Tooltip, Avatar, Rate, message } from "antd";
import { ReplyForm } from "../forms/forms";
import { hasRole, roles } from "../helpers/helpers";

@inject("ReviewStore")
@inject("RestaurantStore")
@inject("AuthStore")
class Review extends React.Component {
    state = {
        showReviewInput: false,
        showReplyInput: false
    };

    delete = async id => {
        const { ReviewStore, RestaurantStore, restaurantId } = this.props;
        try {
            await ReviewStore.Delete(restaurantId, id);

            await RestaurantStore.GetById(restaurantId);
            await ReviewStore.GetReviews(restaurantId);
            await ReviewStore.GetTopAndWorstReview(restaurantId);
        } catch (error) {
            message.error("An error ocurred!");
        }
    };

    deleteReply = async (reviewId, id) => {
        const { ReviewStore, restaurantId } = this.props;
        try {
            await ReviewStore.DeleteReviewReply(restaurantId, reviewId, id);
            await ReviewStore.GetReviews(restaurantId);
        } catch (error) {
            message.error("An error ocurred!");
        }
    };

    showReplyInput = () => {
        this.setState({
            showReplyInput: true
        });
    };

    createReply = async (values, formikBag) => {
        const { restaurantId, review, ReviewStore } = this.props;

        try {
            await ReviewStore.CreateReviewReply(restaurantId, review.id, {
                text: values.text
            });
            formikBag.setSubmitting(false);

            this.setState({
                showReplyInput: false
            });

            await ReviewStore.GetReviewsWithoutReply(restaurantId);
        } catch (error) {
            formikBag.setSubmitting(false);
            formikBag.setStatus(error);
            message.error("An error ocurred!");
        }
    };

    render() {
        const { review, AuthStore, canReply, showActions } = this.props;
        const { showReplyInput } = this.state;
        const isAdmin = hasRole(AuthStore.User, [roles.admin]);
        const isReviewAuthour = review.user.id.localeCompare(AuthStore.User.id) === 0;
        const isReplyAuthor = review.reply == null ? false : review.reply.user.id.localeCompare(AuthStore.User.id) === 0;

        return (
            <>
                <Comment
                    key={review.id}
                    actions={showActions && [
                        <Rate value={review.rating} disabled={true} allowHalf={true} style={{marginRight: "10px"}} />,
                        (isAdmin || isReviewAuthour) && (
                            <span key="comment-nested-reply-to" onClick={() => this.delete(review.id)}>
                                <b>Delete</b>
                            </span>
                        ),
                        !review.reply && canReply && (
                            <span key="comment-nested-reply-to" onClick={() => this.showReplyInput()}>
                                <b>Reply</b>
                            </span>
                        )
                    ]}
                    avatar={<Avatar shape="square" icon="user" alt={review.user && review.user.username} />}
                    content={<p>{review.text}</p>}
                    author={review.user && review.user.username}
                    datetime={
                        <Tooltip title={moment(review.timestamp).format("DD.MM.YYYY HH:mm:ss")}>
                            <span>on {moment(review.timestamp).format("DD.MM.YYYY")}</span>
                        </Tooltip>
                    }
                >
                    {review.reply && (
                        <Comment
                            actions={[
                                (isAdmin || isReplyAuthor) && (
                                    <span key="comment-nested-reply-to" onClick={() => this.deleteReply(review.id, review.reply.id)}>
                                        Delete
                                    </span>
                                )
                            ]}
                            avatar={<Avatar shape="square" icon="user" alt={review.user && review.user.username} />}
                            content={review.reply.text}
                            author={review.reply.user.username}
                            datetime={
                                <Tooltip
                                    title={moment()
                                        .subtract(review.reply.timestamp)
                                        .format("YYYY-MM-DD HH:mm:ss")}
                                >
                                    <span>
                                        {moment()
                                            .subtract(review.reply.timestamp)
                                            .fromNow()}
                                    </span>
                                </Tooltip>
                            }
                        />
                    )}
                </Comment>
                {showReplyInput && <ReplyForm onSubmit={this.createReply} />}
            </>
        )
    }
}

export default Review;
