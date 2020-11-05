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



//users
export const createUser = (login, password) => instance.post(`/user`, { login: login, password: password }).then(result => result.data).catch(errorHandler);
export const loginUser = (login, password) => instance.post(`/authentication`, { login: login, password: password }).then(result => result.data).catch(errorHandler);


//Events
export const getEventsByTrackId = (trackId,take = null, skip = null) => instance.get(`/tracks/${trackId}/events`, { params: { take: take, skip: skip }, headers: authHeader() }).then(result => result.data).catch(errorHandler);

//tracks
export const getTracks = (take = null, skip = null) => instance.get(`/tracks`, { params: { take: take, skip: skip }, headers: authHeader() }).then(result => result.data).catch(errorHandler);
export const deleteTrack = (id) => instance.delete('/tracks/'+id, { headers: authHeader() }).then(result => result.data).catch(errorHandler);
export const createTrack = (trackContent) => instance.post('/tracks/', trackContent,{ headers: authHeader()}).then(result => result.data).catch(errorHandler);
