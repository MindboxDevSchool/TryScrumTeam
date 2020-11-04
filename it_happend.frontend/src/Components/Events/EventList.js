import { makeStyles } from '@material-ui/core/styles';
import React from 'react';
import { useState, useEffect } from "react";
import { getEventsByTrackId } from './../../api';
import EventBox from './EventBox';
import { Button, Typography, LinearProgress } from '@material-ui/core';
import AddCircleOutlineIcon from '@material-ui/icons/AddCircleOutline';

const trackId = 'f22ff20a-d0fc-4dbe-93ac-3b86f28e9c22';

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
        justifyContent: 'flex-start',
        marginTop: theme.spacing(2),
        marginBottom: theme.spacing(4),
    },
    loadButtonContainer: {
        display: 'flex',
        justifyContent: 'center',
    }
}));


export default function Tracks({trackName,AllowedCustomizations}) {
    const classes = useStyles();
    const takeSize = 10;
    const [events, setTracks] = useState([]);
    const [hasNext, setHasNext] = useState(false);
    const [isAddTrackLoading, setAddTrackLoading] = useState(false);
    const [isStartTrackLoading, setStartTrackLoading] = useState(true);

    useEffect(() => {
        const getFirstTracks = async () => {
            const { events } = await getEventsByTrackId(trackId,takeSize);
            setHasNext(Array.isArray(events) && events.length === takeSize)
            setTracks(events);
            setStartTrackLoading(false);
        }
        getFirstTracks();
    }, []);

    const loadNext = async () => {
        setAddTrackLoading(true);
        const addTracks = await getEventsByTrackId(trackId,takeSize, events.length)
        const extendedTracks = events.concat(addTracks.tracks);
        setHasNext(Array.isArray(addTracks.tracks) && addTracks.tracks.length === takeSize)
        setTracks(extendedTracks);
        setAddTrackLoading(false);
    }


    return (
        <>
            <Typography variant="h4" className={classes.title}>
                {trackName}
            </Typography>
            <Typography variant="h4" className={classes.title}>
                {AllowedCustomizations.join("  ")}
            </Typography>
            <div className={classes.buttonContainer}>
                <Button
                    variant="contained"
                    size="large"
                    color="default"
                    startIcon={<AddCircleOutlineIcon />}
                >
                    Добавить событие
                </Button>
            </div>
            {isStartTrackLoading ? <LinearProgress /> :
                (Array.isArray(events) && events.length ?
                    <>
                        {events.map(t => <EventBox createdAt = {t.createdAt} {...t.customizations} />)}
                        {
                            hasNext ?
                                (isAddTrackLoading ? <LinearProgress /> :
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
                        Ничего еще не случилось. Самое время добавить новое событие.
                 </Typography>
                )}
        </>
    );
}