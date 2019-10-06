import axios from "axios";
import {
    observable,
    action,
    computed
} from "mobx";
import { message } from "antd";
import { getAuthHeader } from "../helpers/helpers";
import { apiUrl } from "../config";

class RestaurantStore {
    @observable Restaurants;
    @observable Restaurant;

    @action
    GetAll = async () => {
        try {
            const result = await axios.get(`${apiUrl}/restaurants`, {
                headers: getAuthHeader()
            });
            this.Restaurants = result.data;
            return result.status;
        } catch (error) {
            message.error("An error ocurred!");
        }
    }

    @action
    GetById = async (id) => {
        try {
            const result = await axios.get(`${apiUrl}/restaurants/${id}`, {
                headers: getAuthHeader()
            });
            this.Restaurant = result.data;
        } catch (error) {
            message.error("An error ocurred!");
        }
    }

    @action
    GetByRating = async (rating) => {
        try {
            const result = await axios.get(`${apiUrl}/restaurants?rating=${rating}`, {
                headers: getAuthHeader()
            });

            this.Restaurants = result.data;
            return result.status;
        } catch (error) {
            message.error("An error ocurred!");
        }
    }

    @action
    GetRestaurantsForUser = async () => {
        try {
            const result = await axios.get(`${apiUrl}/restaurants/user`, {
                headers: getAuthHeader()
            });

            this.Restaurants = result.data;
        } catch (error) {
            message.error("An error ocurred!");
        }
    }

    Create = async (restaurant) => {
        const result = await axios.post(`${apiUrl}/restaurants`, restaurant, {
            headers: getAuthHeader()
        });

        return result.data;
    }


    Update = async (id, restaurant) => {
        const result = await axios.put(`${apiUrl}/restaurants/${id}`, restaurant, {
            headers: getAuthHeader()
        });

        return result.data;
    }

    Delete = async (id) => {
        try {
            await axios.delete(`${apiUrl}/restaurants/${id}`, {
                headers: getAuthHeader()
            });
        } catch (error) {
            message.error("An error ocurred!");
        }
    }

    PendingReplyCount = async (id, cancelToken) => {
        const result = await axios.get(`${apiUrl}/restaurants/${id}/pendingreplycount`, {
            headers: getAuthHeader(),
            cancelToken: cancelToken
        });

        return result.data;
    }

    @computed get IsLoading() {
        return this.Restaurants === undefined || this.Restaurants === null;
    }
}

export default new RestaurantStore();