import * as React from "react";
import { Rate, Button } from "antd";
import { withRouter } from "react-router-dom";
import { inject } from "mobx-react";
import { hasRole, roles } from "../helpers/helpers";
import { ReplyButton } from "../components/components";

@inject("RestaurantStore")
class CardActions extends React.PureComponent {

    delete = async (id, deleteCb) => {
        const { RestaurantStore } = this.props;
        try {
            await RestaurantStore.Delete(id);
            await deleteCb();
        } catch (error) {
            console.error(error);
        }
    };

    render() {
        const { User, history, restaurant, deleteCb } = this.props;

        return [
            <React.Fragment key={restaurant.id}>
                <>
                    <Rate value={restaurant.rating} disabled={true} style={{ marginRight: "20px" }} />
                    <Button type="primary" style={{ marginRight: "10px" }} onClick={() => history.push(`/restaurant/${restaurant.id}`)}>
                        Details
                    </Button>
                </>
                {hasRole(User, [roles.owner]) && (
                    <ReplyButton id={restaurant.id} />
                )}
                {hasRole(User, [roles.admin]) && (
                    <Button type="primary" onClick={async () => await this.delete(restaurant.id, deleteCb)}>
                        Delete
                    </Button>
                )}
            </React.Fragment>
        ]
    }
}

export default withRouter(CardActions);