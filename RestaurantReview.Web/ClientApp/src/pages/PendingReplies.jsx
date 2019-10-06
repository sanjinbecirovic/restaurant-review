import * as React from "react";
import { withRouter } from "react-router-dom";
import { inject, observer } from "mobx-react";
import { Review } from "../components/components";
import { Button } from "antd";

@inject("ReviewStore")
@inject("RestaurantStore")
@observer
class PendingReplies extends React.PureComponent {
    constructor(props) {
        super(props);
        const { ReviewStore, RestaurantStore } = props;

        ReviewStore.Reviews = null;
        RestaurantStore.Restaurant = null
    }
    componentDidMount = async () => {
        const { ReviewStore, RestaurantStore, match } = this.props;
        const { id } = match.params;

        await ReviewStore.GetReviewsWithoutReply(id);
        await RestaurantStore.GetById(id);
    }

    render() {
        const { ReviewStore, RestaurantStore, history } = this.props;

        if (!RestaurantStore.Restaurant || !ReviewStore.Reviews) return null;

        return (
            <>
                <Button type="primary" style={{ marginBottom: "10px" }} onClick={() => history.goBack()}>Back</Button>
                {ReviewStore.Reviews.map(review => (
                    <Review key={review.id} review={review} restaurantId={RestaurantStore.Restaurant.id} canReply={true} showActions={true} />
                ))}
            </>
        )
    }
}

export default withRouter(PendingReplies);