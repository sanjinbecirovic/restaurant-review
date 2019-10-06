import * as React from "react";
import { NavLink } from 'react-router-dom';
import { withRouter } from "react-router-dom";
import { Menu, message } from 'antd';
import { inject, observer } from "mobx-react";

@inject("AuthStore")
@observer
class Navigation extends React.Component {
    logout = () => {
        try {
            this.props.AuthStore.Logout();
        } catch(error){
            message.error("An error ocurred");
        }
    }

    render() {
        const { AuthStore } = this.props;

        return (
            <Menu
                theme="dark"
                mode="horizontal"
                style={{ lineHeight: '64px' }}
                selectable={false}
            >
                <Menu.Item key="1">
                    {AuthStore.IsAuthenticated && <NavLink to="/" className="nav-link">Restaurant Review</NavLink>}
                </Menu.Item>
                <Menu.Item key="4" style={{float: "right"}}>
                    {AuthStore.IsAuthenticated && <div className="nav-link" onClick={this.logout}>Logout</div>}
                </Menu.Item>
            </Menu>
        )
    }
}

export default withRouter(Navigation);