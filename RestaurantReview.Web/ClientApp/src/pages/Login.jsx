import React from "react";
import { inject, observer } from "mobx-react";
import { withRouter, Redirect } from "react-router-dom";
import { LoginForm } from "../forms/forms";
import { parseServerErrorResponse, mapServerErrorsToInputFields } from "../helpers/helpers";

@inject("AuthStore")
@observer
class Login extends React.PureComponent {
    login = async (values, formikBag) => {
        const { AuthStore, history } = this.props;
        try {
            await AuthStore.Login(values.username, values.password);
            formikBag.setSubmitting(false);

            await AuthStore.Authenticate();
            history.push("/");
        } catch (error) {
            const errors = parseServerErrorResponse(error.response);
            if (errors) {
                mapServerErrorsToInputFields(errors, formikBag);
            }
            if (errors && errors.isLockedOut && errors.isLockedOut.length > 0) {
                formikBag.setStatus(errors.isLockedOut[0]);
            }
            formikBag.setSubmitting(false);
        }
    }

    render() {
        const { AuthStore } = this.props;

        if (AuthStore.IsFetching) {
            return null;
        }

        if (AuthStore.User) {
            return <Redirect to="/" />
        }

        return (
            <LoginForm onSubmit={this.login} />
        )
    }
}

export default withRouter(Login);
