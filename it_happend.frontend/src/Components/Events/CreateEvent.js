import { makeStyles } from '@material-ui/core/styles';
import React from 'react';
import { useState, useEffect } from "react";
import { Typography } from '@material-ui/core';
import { useParams } from "react-router-dom";
import EventForm from './EventForm';
import { createEvent } from '../../api';
import history from '../../history' 

const useStyles = makeStyles((theme) => ({
    title: {
        marginTop: theme.spacing(3),
    },
}));


export default function CreateEvent() {
    const { trackId } = useParams();
    const classes = useStyles();
    //track fields { id, name, createdAt, allowedCustomizations } 
    const [track, setTrack] = useState({});

    useEffect(() => {
        var savedTrack = JSON.parse(localStorage.getItem('track'));
        if (savedTrack && savedTrack.id === trackId) {
            setTrack(savedTrack);
        }
        else {
            setTrack({ name: "name", allowedCustomizations: ["default"] })
            //get track from api
        }
    }, [trackId]);

    const saveNewEvent = async (eventContent) => {
        var result = await createEvent(trackId, eventContent)
        if (result)
            history.push(`/tracks/${trackId}`)
    }

    return (
        <>
            <Typography variant="h4" className={classes.title}>
                Добавление событие для {track.name}
            </Typography>
            <EventForm allowedCustomizations={track.allowedCustomizations} onSave={saveNewEvent} />
        </>
    )
}