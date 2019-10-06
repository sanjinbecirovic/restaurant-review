import * as React from "react";
import axios from "axios";
import { Button, Badge } from "antd";
import { withRouter } from "react-router-dom";
import { inject } from "mobx-react";

@inject("RestaurantStore")
class ReplyButton extends React.Component {
    state = {
        replyCount: 0
    }

    constructor(props) {
        super(props);
        this.counter = null;
        this.source = axios.CancelToken.source();
    }

    async componentDidMount() {
        await this.getCount();
        this.counter = setTimeout(this.getCount, 10000);
    }

    componentWillUnmount() {
        this.source.cancel();
        if (this.counter) {
            clearTimeout(this.counter);
        }
    }

    getCount = async () => {
        const { id, RestaurantStore } = this.props;

        let count = 0;
        try {
            count = await RestaurantStore.PendingReplyCount(id, this.source.token);
            this.setState({
                replyCount: count,
            });
        } catch (error) {}
    }

    render() {
        const { history, id } = this.props;
        const { replyCount } = this.state;

        return (
            <Badge count={replyCount} offset={[-15,0]}>
                <Button type="primary" style={{ marginRight: "10px" }} onClick={() => history.push(`/restaurant/${id}/replies`)}>
                    Replies
                </Button>
            </Badge>
        )
    }
}

export default withRouter(ReplyButton);