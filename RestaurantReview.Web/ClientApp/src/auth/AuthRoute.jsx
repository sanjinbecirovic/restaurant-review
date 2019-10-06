import React from 'react';
import { hasRole } from "../helpers/helpers";
import { toJS } from 'mobx';
import { inject, observer } from "mobx-react";
import {
  withRouter,
  Route,
  Redirect,
} from 'react-router-dom'

@inject("AuthStore")
@observer
class AuthRoute extends React.PureComponent {

  render() {
    const { component: Component, AuthStore, roles, location, path, ...rest } = this.props;

    if(AuthStore.IsFetching) {
      return null;
    }

    if (!AuthStore.IsAuthenticated) {
      return <Redirect to="/login" />;
    }

    if (hasRole(toJS(AuthStore.User), roles)) {
      return <Route
        {...rest}
        render={(props) => {
          return <Component {...props} />
        }} />
    }

    return <Redirect to="/" />;
  }
}

export default withRouter(AuthRoute);