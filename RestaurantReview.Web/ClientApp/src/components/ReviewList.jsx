import React from "react";
import { withRouter } from "react-router-dom";
import { inject, observer } from "mobx-react";
import { Review } from "../components/components";
import { hasRole, roles } from "../helpers/helpers";

@inject("AuthStore")
@inject("ReviewStore")
@observer
class ReviewList extends React.PureComponent {
    componentDidMount = async () => {
        const { ReviewStore, restaurantId } = this.props;
        ReviewStore.Reviews = null;
        await ReviewStore.GetReviews(restaurantId);
    }

    render() {
        const { ReviewStore, AuthStore, restaurantId, canReply, showActions } = this.props;
        const isAdmin = hasRole(AuthStore.User, [roles.admin]);

        if (!ReviewStore.Reviews) return null;

        return (
            <>
                <div className="ant-descriptions-title">Latest reviews</div>
                {ReviewStore.Reviews.map(review => (
                    <Review key={review.id} review={review} restaurantId={restaurantId} isAdmin={isAdmin} canReply={canReply} showActions={showActions}/>))}
            </>

        );
    }
}

export default withRouter(ReviewList);
