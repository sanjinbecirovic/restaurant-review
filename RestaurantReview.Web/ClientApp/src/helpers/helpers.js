import * as ls from "local-storage";
import * as HttpStatus from 'http-status-codes';
import { message } from "antd";

export const hasRole = (user, roles) => {
    if (roles === undefined || roles.length === 0) return true;
    if (user.roles === undefined || user.roles.length === 0) return false;

    const userRoles = user.roles.map(role => role.toLowerCase());
    return roles.some(role => userRoles.includes(role.toLowerCase()));
}

export const roles = {
    owner: "restaurant owner",
    admin: "admin"
}

export function getAuthHeader() {
    return {
        "Authorization": `Bearer ${ls.get("restaurant-review")}`
    };
}

export function parseServerErrorResponse(response) {
    if (!response) {
        message.error("An error ocurred!");
        return null;
    }

    switch (response.status) {
        case HttpStatus.INTERNAL_SERVER_ERROR:
            message.error("An error ocurred!");
            return null;
        case HttpStatus.BAD_REQUEST:
            return response.data;
        default:
            return null;
    }
}

export function mapServerErrorsToInputFields(errors, formikBag) {
    for (var error in errors) {
        if (Object.prototype.hasOwnProperty.call(errors, error)) {
            formikBag.setFieldError(error.toString(), errors[error]);
        }
    }
}