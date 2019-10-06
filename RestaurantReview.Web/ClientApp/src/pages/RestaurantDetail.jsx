import * as React from "react";
import { inject, observer } from "mobx-react";
import { Descriptions, Rate, Button, message } from "antd";
import { withRouter } from "react-router";
import { Review, ReviewList } from "../components/components";
import { ReviewForm } from "../forms/forms";
import { hasRole, roles } from "../helpers/helpers";

@inject("RestaurantStore")
@inject("ReviewStore")
@inject("AuthStore")
@observer
class RestaurantDetail extends React.PureComponent {
    constructor(props) {
        super(props);
        const { RestaurantStore, ReviewStore } = props;

        RestaurantStore.Restaurant = null;
        ReviewStore.Reviews = null;
        ReviewStore.TopReview = null;
        ReviewStore.WorstReview = null;

        this.state = {
            hideReviewInput: false
        };
    }

    componentDidMount = async () => {
        const { RestaurantStore, ReviewStore } = this.props;
        const { id } = this.props.match.params;

        await RestaurantStore.GetById(id);
        await ReviewStore.GetTopAndWorstReview(id);
    };

    componentWillUnmount() {
        const { RestaurantStore, ReviewStore } = this.props;

        RestaurantStore.Restaurant = null;
        ReviewStore.Reviews = null;
        ReviewStore.TopReview = null;
        ReviewStore.WorstReview = null;
    }

    deleteReviewCb = async () => {
        this.setState({
            hideReviewInput: false
        });
        const { RestaurantStore, ReviewStore } = this.props;
        const { id } = this.props.match.params;

        await RestaurantStore.GetById(id);
        await ReviewStore.GetTopAndWorstReview(id);
        await ReviewStore.GetReviews(id);
    };

    lockOwner = async (userId, enabled) => {
        const { AuthStore, RestaurantStore } = this.props;

        try {
            await AuthStore.LockoutUser(userId, enabled);
            await RestaurantStore.GetById(RestaurantStore.Restaurant.id);

            if (enabled) {
                message.success("Successfully locked!");
            } else {
                message.success("Successfully unlocked!");
            }
        } catch (error) {
            message.error("An error ocurred!");
        }
    }

    render() {
        const { Restaurant } = this.props.RestaurantStore;
        const { User } = this.props.AuthStore;
        const { TopReview, WorstReview } = this.props.ReviewStore;
        const { history } = this.props;
        const { hideReviewInput } = this.state;

        if (!Restaurant) return null;

        const isOwner = Restaurant.userId.localeCompare(User.id) === 0;
        const alreadyReviewed = Restaurant.reviews.filter(review => review.user.id.localeCompare(User.id) === 0).length > 0;
        const isLockoutEnabled = hasRole(User, [roles.admin]) && Restaurant.user.lockoutEnd;
        const isLockoutDisabled = hasRole(User, [roles.admin]) && !Restaurant.user.lockoutEnd;
        const canEdit = hasRole(User, [roles.admin]) || isOwner;
        const columnSpan = hasRole(User, [roles.admin]) ? 3 : 2;

        return (
            <>
                <div style={{ marginBottom: "10px" }} >
                    <Button type="primary" onClick={() => history.goBack()}>Back</Button>
                    {isLockoutDisabled &&
                        <Button style={{ float: "right", marginRight: "10px" }} type="primary" onClick={() => this.lockOwner(Restaurant.user.id, true)}>Lock Owner</Button>
                    }
                    {isLockoutEnabled &&
                        <Button type="primary" style={{ float: "right", marginRight: "10px" }} onClick={() => this.lockOwner(Restaurant.user.id, false)}>Unlock Owner</Button>
                    }
                    {canEdit &&
                        <Button type="primary" style={{ float: "right", marginRight: "10px" }} onClick={() => history.push(`/restaurant/${Restaurant.id}/edit`)}>
                            Edit
                        </Button>
                    }
                </div>
                <Descriptions title={Restaurant.name} column={columnSpan} style={{ margin: "0 0 50px" }}>
                    <Descriptions.Item label="Address">
                        {Restaurant.address}
                    </Descriptions.Item>
                    {hasRole(User, [roles.admin]) &&
                        <Descriptions.Item label="Owner">
                            {Restaurant.user.username}
                        </Descriptions.Item>
                    }
                    <Descriptions.Item label="Rating" style={{textAlign: "center"}}>
                        <Rate value={Restaurant.rating} disabled={true} allowHalf={true} />
                    </Descriptions.Item>
                    <Descriptions.Item label="Description" span={columnSpan}>
                        {Restaurant.description}
                    </Descriptions.Item>
                </Descriptions>
                {!isOwner && !alreadyReviewed && !hideReviewInput && (
                    <>
                        <div className="ant-descriptions-title">Write a review</div>
                        <ReviewForm restaurantId={Restaurant.id} cb={this.deleteReviewCb} />
                    </>
                )}
                {TopReview && (
                    <>
                        <div className="ant-descriptions-title">Top Review</div>
                        <Review review={TopReview} showActions={false} />
                    </>
                )}
                {WorstReview && (
                    <>
                        <div className="ant-descriptions-title">Worst Review</div>
                        <Review review={WorstReview} showActions={false} />
                    </>
                )}
                <ReviewList restaurantId={Restaurant.id} canReply={false} showActions={true} />
            </>
        );
    }
}

export default withRouter(RestaurantDetail);
