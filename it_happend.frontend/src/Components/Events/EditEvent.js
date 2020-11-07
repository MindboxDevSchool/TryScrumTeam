import { makeStyles } from '@material-ui/core/styles';
import React from 'react';
import { useState, useEffect } from "react";
import { Typography } from '@material-ui/core';
import { useParams } from "react-router-dom";
import EventForm from './EventForm';
import { editEvent } from '../../api';
import history from '../../history'; 
import moment from 'moment'
import 'moment/locale/ru'
moment.locale('ru')

const useStyles = makeStyles((theme) => ({
    title: {
        marginTop: theme.spacing(3),
    },
}));


export default function EditEvent() {
    const { trackId, eventId } = useParams();
    const classes = useStyles();
    //track fields { id, name, createdAt, allowedCustomizations } 
    const [track, setTrack] = useState({});
    const [event, setEvent] = useState({});
    const [allowedCustomizations, setCustomizations] = useState([]);
    useEffect(() => {
        var savedTrack = JSON.parse(localStorage.getItem('track'));
        if (savedTrack && savedTrack.id === trackId) {
            setTrack(savedTrack);
        }
        else {
            setTrack({ name: "name", allowedCustomizations: ["default"] })
            //get track from api
        }
        var savedEvent = JSON.parse(localStorage.getItem('event'));
        if (savedEvent && savedEvent.id === eventId) {
            setEvent(savedEvent);
        }
        else {
            setEvent({ CreatedAt: new Date() })
            //get event from api
        }
        let customizations = savedTrack.allowedCustomizations;
        if (savedEvent["comment"])
            customizations.push("Comment")
        if (savedEvent["rating"])
            customizations.push("Rating")
        if (savedEvent["scale"])
            customizations.push("Scale")
        if (savedEvent["photoUrl"])
            customizations.push("Photo")
        if (savedEvent["geotagLatitude"])
            customizations.push("Geotag")
        setCustomizations(customizations);
    }, [trackId, eventId]);

    const saveNewEvent = async (eventContent) => {
        var result = await editEvent(trackId, eventId, eventContent)
        if (result)
            history.push(`/tracks/${trackId}`)
    }

    return (
        <>
            <Typography variant="h4" className={classes.title}>
                Редактирование события для {track.name}
            </Typography>
            {event.id ?
                <>
                    <Typography variant="h6" className={classes.title}>
                        Дата события : {moment(event.CreatedAt).format('LLLL')}
                    </Typography>
                    <EventForm isEdit={true} allowedCustomizations={allowedCustomizations} event={event} onSave={saveNewEvent} />
                </>
                :
                null
            }
        </>
    )
}