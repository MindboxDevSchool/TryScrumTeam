import axios from "axios";
import history from './history' 
const apiUrl = "http://localhost:5000";

const instance = axios.create({
    baseURL: apiUrl,
})

function errorHandler(error) {
    if (error.response) {
        if (error.response.status === 404) {
            history.push('/404');
        } 
        if (error.response.status === 401) {
            localStorage.removeItem('token');
            localStorage.removeItem('login');
            window.location.reload(false);
        } 
        if (error.response.statis >= 500)
            window.alert("Что-то пошло не так, пожалуйста обновите страницу или попробуйте позже!");
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
export const deleteEvent = (trackId,eventId) => instance.delete(`/tracks/${trackId}/events/${eventId}`, { headers: authHeader() }).then(result => result.data).catch(errorHandler);


//tracks
export const getTracks = (take = null, skip = null) => instance.get(`/tracks`, { params: { take: take, skip: skip }, headers: authHeader() }).then(result => result.data).catch(errorHandler);
export const deleteTrack = (id) => instance.delete('/tracks/'+id, { headers: authHeader() }).then(result => result.data).catch(errorHandler);
export const createTrack = (trackContent) => instance.post('/tracks/', trackContent,{ headers: authHeader()}).then(result => result.data).catch(errorHandler);
export const editTrack = (trackContent,id) => instance.put('/tracks/'+id, trackContent,{ headers: authHeader()}).then(result => result.data).catch(errorHandler);
export const getTrackStatistics = (id) => instance.get(`/tracks/${id}/statistics`, { headers: authHeader() }).then(result => result.data).catch(errorHandler);

