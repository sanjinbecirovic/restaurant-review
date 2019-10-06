import React from "react";
import { inject, observer } from "mobx-react";
import { withRouter, Redirect } from "react-router-dom";
import { Button } from "antd";
import RegisterForm from "../forms/RegisterForm";
import { roles } from "../helpers/helpers";
import { parseServerErrorResponse, mapServerErrorsToInputFields } from "../helpers/helpers";

@inject("AuthStore")
@observer
class Register extends React.PureComponent {

    register = async (values, formikBag) => {
        const { history, AuthStore } = this.props;

        try {
            const role = values.isOwner ? roles.owner : "";
            await AuthStore.Register(values.email, values.password, role);
            formikBag.setSubmitting(false);

            await AuthStore.Authenticate();
            history.push("/");
        } catch (error) {
            const errors = parseServerErrorResponse(error.response);
            if (errors) {
                mapServerErrorsToInputFields(errors, formikBag);
            }
            formikBag.setSubmitting(false);
        }
    }

    render() {
        const { AuthStore, history } = this.props;

        if (AuthStore.IsFetching) {
            return null;
        }

        if (AuthStore.User) {
            return <Redirect to="/" />
        }

        return (
            <>
                <Button type="primary" onClick={() => history.goBack()}>Back</Button>
                <RegisterForm onSubmit={this.register} />
            </>
        )
    }
}

export default withRouter(Register);
