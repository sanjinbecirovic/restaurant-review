import axios from "axios";
import { observable, action } from "mobx";
import {
    getAuthHeader
} from "../helpers/helpers";
import { message } from "antd";
import { apiUrl } from "../config";

class ReviewStore {
    @observable Reviews;
    @observable TopReview;
    @observable WorstReview;

    @action
    GetReviews = async (restaurantId) => {
        try {
            const result = await axios.get(`${apiUrl}/restaurant/${restaurantId}/reviews`, {
                headers: getAuthHeader()
            });
            this.Reviews = result.data;
        } catch (error) {
            message.error("An error ocurred!");
        }
    }

    @action
    GetTopAndWorstReview = async (restaurantId) => {
        try {
            const topResult = await axios.get(`${apiUrl}/restaurant/${restaurantId}/reviews/top`, {
                headers: getAuthHeader()
            });

            const worstResult = await axios.get(`${apiUrl}/restaurant/${restaurantId}/reviews/worst`, {
                headers: getAuthHeader()
            });

            this.TopReview = topResult.data;
            this.WorstReview = worstResult.data ;
        } catch (error) {
            message.error("An error ocurred!");
        }
    }

    @action
    GetReviewsWithoutReply = async (restaurantId) => {
        try {
            const result = await axios.get(`${apiUrl}/restaurant/${restaurantId}/reviews?withoutReply=1`, {
                headers: getAuthHeader()
            });

            this.Reviews = result.data;
        } catch (error) {
            message.error("An error ocurred!");
        }
    }

    @action
    Create = async (restaurantId, review) => {
        await axios.post(`${apiUrl}/restaurant/${restaurantId}/reviews/`, review, {
            headers: getAuthHeader()
        })
    }

    @action
    Delete = async(restaurantId, reviewId) => {
        try {
            await axios.delete(`${apiUrl}/restaurant/${restaurantId}/reviews/${reviewId}`, {
                headers: getAuthHeader()
            });
            this.Reviews.remove(this.Reviews.find(review => review.id === reviewId));
            this.GetTopAndWorstReview(restaurantId);
        } catch (error) {
            message.error("An error ocurred!");
        }
    }

    @action
    CreateReviewReply = async (restaurantId, reviewId, reply) => {
        try {
            await axios.post(`${apiUrl}/restaurant/${restaurantId}/reviews/${reviewId}/replies`, reply, {
                headers: getAuthHeader()
            });
        } catch (error) {
            message.error("An error ocurred!");
        }

    }

    @action
    DeleteReviewReply = async (restaurantId, reviewId, replyId) => {
        try {
            await axios.delete(`${apiUrl}/restaurant/${restaurantId}/reviews/${reviewId}/replies/${replyId}`, {
                headers: getAuthHeader()
            });
            const review = this.Reviews.find(review => review.id === reviewId);
            review.reply = null;
        } catch (error) {
            message.error("An error ocurred!");
        }
    }
}

export default new ReviewStore();