import axios from "axios";
import * as ls from "local-storage";
import { observable, action, computed } from "mobx";
import { getAuthHeader } from "../helpers/helpers";
import { apiUrl } from "../config";

class AuthStore {
    @observable User = null;
    @observable IsFetching = false;
    localStorageKey = "restaurant-review";

    constructor() {
        if (this.User == null && ls.get(this.localStorageKey) !== null) {
            this.Authenticate();
        }
    }

    Login = async (username, password) => {
        const result = await axios.post(`${apiUrl}/auth/login`, {
            username: username,
            password: password
        });

        ls.set(this.localStorageKey, result.data.token);
    };

    @action
    Logout = async () => {
        ls.remove(this.localStorageKey);
        this.User = null;
    };

    @action
    Authenticate = async () => {
        try {
            this.IsFetching = true;
            const result = await axios.get(`${apiUrl}/users/me`, {
                headers: getAuthHeader()
            });

            this.User = result.data;
            this.IsFetching = false;
        } catch (error) {
            this.User = null;
            this.IsFetching = false;
        }
    };

    @computed get IsAuthenticated() {
        return this.User != null;
    }

    Register = async (email, password, role) => {
        const result = await axios.post(`${apiUrl}/auth/register`, {
            email,
            password,
            role
        });
        ls.set(this.localStorageKey, result.data.token);
    };

    LockoutUser = async (userId, enabled) => {
        await axios.post(`${apiUrl}/auth/lockout`, {
            userId: userId,
            enabled: enabled
        }, {
            headers: getAuthHeader()
        });
    }
}

export default new AuthStore();
