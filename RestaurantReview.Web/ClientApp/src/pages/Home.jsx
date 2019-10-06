import * as React from "react";
import { inject, observer } from "mobx-react";
import { Card, Rate, Button } from "antd";
import { withRouter } from "react-router-dom";
import { hasRole, roles } from "../helpers/helpers";
import { CardActions } from "../components/components";

const desc = ["terrible", "bad", "normal", "good", "wonderful"];

@inject("AuthStore")
@inject("RestaurantStore")
@observer
class Home extends React.PureComponent {
    constructor(props) {
        super(props);
        const { RestaurantStore } = this.props;

        RestaurantStore.Restaurants = null;

        this.state = {
            rating: 0
        };
    }

    async componentDidMount() {
        await this.loadRestaurants();
    }

    componentWillUnmount() {
        const { RestaurantStore } = this.props;
        RestaurantStore.Restaurants = null;
    }

    loadRestaurants = async () => {
        const { RestaurantStore, AuthStore } = this.props;
        const isOwner = hasRole(AuthStore.User, [roles.owner]);

        if (isOwner) {
            await RestaurantStore.GetRestaurantsForUser();
        } else {
            await RestaurantStore.GetAll();
        }
    }

    filterRestaurants = async rating => {
        const { RestaurantStore } = this.props;
        await RestaurantStore.GetByRating(rating);
        this.setState({
            rating: rating
        });
    };

    clearFilter = async () => {
        this.setState({
            rating: 0
        });

        const { RestaurantStore } = this.props;
        await RestaurantStore.GetAll();
    }

    create = async () => {
        const { history } = this.props;
        history.push("/restaurant/create");
    };

    render() {
        const { AuthStore, RestaurantStore } = this.props;
        const { rating } = this.state;
        const isOwner = hasRole(AuthStore.User, [roles.owner]);
        const isAdmin = hasRole(AuthStore.User, [roles.admin]);

        if (!RestaurantStore.Restaurants) return null;

        return (
            <>
                {!isOwner && (

                    <div style={{ marginBottom: "25px" }}>
                        <span style={{ marginRight: "10px" }}>Filter restaurants by rating</span>
                        <span>
                            <Rate tooltips={desc} onChange={this.filterRestaurants} value={rating} />
                            {rating ? <span className="ant-rate-text">{desc[rating - 1]}</span> : ""}
                        </span>
                        <Button type="primary" style={{ float: "right", marginRight: "10px" }} onClick={() => this.clearFilter()}>
                            Clear filter
                        </Button>
                    </div>
                )}
                {(isOwner || isAdmin) && (
                    <Button type="primary" style={{marginBottom: "20px"}} onClick={() => this.create()}>
                        Create
                    </Button>
                )}
                {RestaurantStore.Restaurants.map(restaurant => {
                    return (
                        <Card
                            style={{ marginBottom: "20px" }}
                            key={restaurant.id}
                            title={restaurant.name}
                            extra={<CardActions key={restaurant.id} User={AuthStore.User} restaurant={restaurant} deleteCb={this.loadRestaurants} />}
                        >
                            <p>{restaurant.description}</p>
                        </Card>
                    );
                })}
            </>
        );
    }
}

export default withRouter(Home);
