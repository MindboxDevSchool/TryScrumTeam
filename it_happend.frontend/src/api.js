import axios from "axios";

const apiUrl = "http://localhost:5000";

const instance = axios.create({
    baseURL: apiUrl,
})

function errorHandler(error) {
    if (error.response) {

    }
    else {
        window.alert("Что-то пошло не так, пожалуйста обновите страницу или попробуйте позже!");
    }
}

function authHeader() {
    return {
        authorization: "Bearer " + localStorage.getItem("token")
    }
};

const getUserInfo = () => ({ name: localStorage.getItem("login"), token: localStorage.getItem("token") });
const auth = { headers: { Authorization: getUserInfo().token } };

//users
export const createUser = (login, password) => instance.post(`/user`, { login: login, password: password }).then(result => result.data).catch(errorHandler);
export const loginUser = (login, password) => instance.post(`/authentication`, { login: login, password: password }).then(result => result.data).catch(errorHandler);

//Events
export const getEventsByTrackId = (trackId,take = null, skip = null) => instance.get(`/tracks/${trackId}/events`, { params: { take: take, skip: skip }, headers: authHeader() }).then(result => result.data).catch(errorHandler);