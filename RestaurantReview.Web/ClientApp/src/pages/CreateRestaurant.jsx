import * as React from "react";
import { inject } from "mobx-react";
import { withRouter } from "react-router-dom";
import { Button } from "antd";
import { RestaurantForm } from "../forms/forms";
import { parseServerErrorResponse, mapServerErrorsToInputFields } from "../helpers/helpers";


@inject("RestaurantStore")
class CreateRestaurant extends React.PureComponent {
    onSubmit = async (values, formikBag) => {
        try {
            const result = await this.props.RestaurantStore.Create({ name: values.name, address: values.address, description: values.description });
            formikBag.setSubmitting(false);
            this.props.history.push(`/restaurant/${result.id}`);
        } catch (error) {
            const errors = parseServerErrorResponse(error.response);
            if (errors) {
                mapServerErrorsToInputFields(errors, formikBag);
            }
            formikBag.setSubmitting(false);
        }
    }

    getInitialValues = () => {
        return {
            name: "",
            address: "",
            description: ""
        }
    }

    render() {
        const {
            history
        } = this.props;

        return (
            <>
                <Button onClick={() => history.goBack()}>Back</Button>
                <RestaurantForm onSubmit={this.onSubmit} getInitialValues={this.getInitialValues} />
            </>
        )
    }
}

export default withRouter(CreateRestaurant);