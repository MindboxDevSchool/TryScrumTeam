import axios from "axios";

const ApiKey = "5729931d6cbf28fc06368abc4b17c19ec32ed4e5";

const instanceAddressSuggestions = axios.create({
    baseURL: "https://suggestions.dadata.ru/suggestions/api/4_1/rs/suggest/address",
    headers: {
        "Content-Type": "application/json",
        "Accept": "application/json",
        "Authorization": "Token " + ApiKey
    },
})

function errorHandler(error) {
    if (error.response) {
        if (error.response.statis >= 500)
            window.alert("Что-то пошло не так, пожалуйста обновите страницу или попробуйте позже!");
    }
    else {
        window.alert("Что-то пошло не так, пожалуйста обновите страницу или попробуйте позже!");
    }
}

export const getAddressSuggestions = (query) => instanceAddressSuggestions.post(``, { query: query }).then(result => result.data).catch(errorHandler);

const bingKey = "ArZtRFi2ge5JPfSDKmilVTjuyz3TlAC9Y1PdG5TMqe7mfQJCvVd5szmv8ylH913Z"
const insanceBing = axios.create({    
})
export const getBingGeotag = (query) => insanceBing.get(`http://dev.virtualearth.net/REST/v1/Locations/${query}?key=${bingKey}`).then(result => result.data).catch(errorHandler);