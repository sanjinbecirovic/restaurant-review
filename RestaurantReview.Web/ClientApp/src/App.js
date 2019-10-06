import "./App.css";

import React, { Component } from "react";
import { Layout } from "antd";
import { Switch, Route } from "react-router-dom";

import Navigation from "./layout/Navigation";
import {
    Login,
    Register,
    CreateRestaurant,
    EditRestaurant,
    RestaurantDetail,
    Home,
    NoMatch,
    PendingReplies
} from "./pages/pages";

import AuthRoute from "./auth/AuthRoute";
import { roles } from "./helpers/helpers";

const { Header, Content, Footer } = Layout;

export default class App extends Component {
    static displayName = App.name;

    render() {
        return (
            <Layout className="layout" style={{ minHeight: "100vh" }}>
                <Header>
                    <div className="logo" />
                    <Navigation />
                </Header>
                <Content style={{ padding: "0 50px" }}>
                    <div style={{ background: "#fff", padding: 24, minHeight: "100vh" }}>
                        <Switch>
                            <Route path="/login" component={Login} exact />
                            <Route path="/register" component={Register} exact />
                            <AuthRoute path="/restaurant/create" component={CreateRestaurant} roles={[roles.owner, roles.admin]} strict />
                            <AuthRoute path="/restaurant/:id/replies" component={PendingReplies} roles={[roles.owner]} exact />
                            <AuthRoute path="/restaurant/:id/edit" component={EditRestaurant} roles={[roles.owner, roles.admin]} strict />
                            <AuthRoute path="/restaurant/:id" component={RestaurantDetail} strict />
                            <AuthRoute path="/" component={Home} exact />
                            <Route component={NoMatch} />
                        </Switch>
                    </div>
                </Content>
                <Footer style={{ textAlign: "center" }}>© Copyright 2010 - 2019 Toptal, LLC</Footer>
            </Layout>
        );
    }
}
