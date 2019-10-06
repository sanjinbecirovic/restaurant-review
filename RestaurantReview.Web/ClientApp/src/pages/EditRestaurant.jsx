import * as React from "react";
import { inject, observer } from "mobx-react";
import { withRouter } from "react-router-dom";
import { Button } from "antd";
import { RestaurantForm } from "../forms/forms";
import { parseServerErrorResponse, mapServerErrorsToInputFields } from "../helpers/helpers";


@inject("RestaurantStore")
@observer
class EditRestaurant extends React.PureComponent {
    constructor(props) {
        super(props);
        const { RestaurantStore } = props;

        RestaurantStore.Restaurant = null;
    }

    async componentDidMount() {
        const { RestaurantStore } = this.props;
        const { id } = this.props.match.params;

        await RestaurantStore.GetById(id);
    }

    componentWillUnmount() {
        const { RestaurantStore } = this.props;
        RestaurantStore.Restaurant = null;
    }

    onSubmit = async (values, formikBag) => {
        const { RestaurantStore, history } = this.props;
        try {
            const result = await RestaurantStore.Update(
                RestaurantStore.Restaurant.id,
                {
                    name: values.name,
                    address: values.address,
                    description: values.description
                });

            formikBag.setSubmitting(false);
            history.push(`/restaurant/${result.id}`);
        } catch (error) {
            const errors = parseServerErrorResponse(error.response);
            if (errors) {
                mapServerErrorsToInputFields(errors, formikBag);
            }
            formikBag.setSubmitting(false);
        }
    }


    getInitialValues = () => {
        const {
            Restaurant
        } = this.props.RestaurantStore;

        return {
            name: Restaurant.name,
            address: Restaurant.address,
            description: Restaurant.description
        }
    }

    render() {
        const {
            RestaurantStore,
            history
        } = this.props;

        if (!RestaurantStore.Restaurant) return null;

        return (
            <>
                <Button type="primary" onClick={() => history.goBack()}>Back</Button>
                <RestaurantForm onSubmit={this.onSubmit} getInitialValues={this.getInitialValues} />
            </>
        )
    }
}

export default withRouter(EditRestaurant);