import { makeStyles } from '@material-ui/core/styles';
import React from 'react';
import { useState, useEffect } from "react";
import { getEventsByTrackId } from './../../api';
import EventBox from './EventBox';
import { Button, Typography, LinearProgress } from '@material-ui/core';
import AddCircleOutlineIcon from '@material-ui/icons/AddCircleOutline';

const trackId = '0fe20c33-680b-4754-8383-2ef7b0d27b45';

const useStyles = makeStyles((theme) => ({
    title: {
        marginTop: theme.spacing(3),
    },
    emptyTracks: {
        textAlign: 'center',
        marginTop: theme.spacing(3),
    },
    buttonContainer: {
        display: 'flex',
        justifyContent: 'flex-end',
        marginTop: theme.spacing(2),
        marginBottom: theme.spacing(4),
    },
    loadButtonContainer: {
        display: 'flex',
        justifyContent: 'center',
    }
}));


export default function Events() {
    const classes = useStyles();
    const takeSize = 10;
    const [events, setEvents] = useState([]);
    const [hasNext, setHasNext] = useState(false);
    const [isAddEventLoading, setAddEventLoading] = useState(false);
    const [isStartEventLoading, setStartEventLoading] = useState(true);

    useEffect(() => {
        const getFirstTracks = async () => {
            const { events } = await getEventsByTrackId(trackId,takeSize);
            setHasNext(Array.isArray(events) && events.length === takeSize)
            setEvents(events);
            setStartEventLoading(false);
        }
        getFirstTracks();
    }, []);

    const loadNext = async () => {
        setAddEventLoading(true);
        const addEvents = await getEventsByTrackId(takeSize, events.length)
        const extendedTracks = events.concat(addEvents.tracks);
        setHasNext(Array.isArray(addEvents.tracks) && addEvents.tracks.length === takeSize)
        setEvents(extendedTracks);
        setAddEventLoading(false);
    }

    return (
        <>
            <Typography variant="h4" className={classes.title}>
                Отслеживания
            </Typography>
            <div className={classes.buttonContainer}>
                <Button
                    variant="contained"
                    size="large"
                    color="default"
                    startIcon={<AddCircleOutlineIcon />}
                >
                    Добавить отслеживание
                </Button>
            </div>
            {isStartEventLoading ? <LinearProgress /> :
                (Array.isArray(events) && events.length ?
                    <>
                        {events.map(t => <EventBox {...t} />)}
                        {
                            hasNext ?
                                (isAddEventLoading ? <LinearProgress /> :
                                    <div className={classes.loadButtonContainer}>
                                        <Button
                                            variant="contained"
                                            color="default"
                                            onClick={loadNext}
                                        >
                                            Загрузить еще
                                    </Button>
                                    </div>
                                ) :
                                null
                        }
                    </>
                    :
                    <Typography variant="h6" className={classes.emptyTracks}>
                        У тебя пока нет отслеживаний. Попробуй добавить новое
                 </Typography>
                )}
        </>
    );
}