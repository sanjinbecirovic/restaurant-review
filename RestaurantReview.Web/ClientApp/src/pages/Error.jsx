import React from 'react'
import { Result, Button } from 'antd';
import { withRouter } from "react-router-dom";

class Error extends React.PureComponent {
    redirectToHome = () => {
        this.props.history.push("/");
    }

    render() {
        return <Result
            status="500"
            title="500"
            subTitle="Sorry, something went wrong."
            extra={<Button type="primary" onClick={this.redirectToHome}>Back Home</Button>} />
    }
}

export default withRouter(Error);

