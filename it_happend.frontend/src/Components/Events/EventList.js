import { makeStyles } from '@material-ui/core/styles';
import React from 'react';
import { useState, useEffect } from "react";
import { getEventsByTrackId } from './../../api';
import EventBox from './EventBox';
import { Button, Typography, LinearProgress } from '@material-ui/core';
import AddCircleOutlineIcon from '@material-ui/icons/AddCircleOutline';
import { useParams } from "react-router-dom";
import { Link } from "react-router-dom"

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


export default function Tracks() {
    const { trackId } = useParams();
    const classes = useStyles();
    const takeSize = 10;

    //track fields { id, name, createdAt, allowedCustomizations } 
    const [track, setTrack] = useState({});
    const [events, setTracks] = useState([]);
    const [hasNext, setHasNext] = useState(false);
    const [isAddTrackLoading, setAddTrackLoading] = useState(false);
    const [isStartTrackLoading, setStartTrackLoading] = useState(true);

    useEffect(() => {
        const getFirstTracks = async () => {
            const { events } = await getEventsByTrackId(trackId, takeSize);
            setHasNext(Array.isArray(events) && events.length === takeSize)
            setTracks(events);
            setStartTrackLoading(false);
        }

        var savedTrack = JSON.parse(localStorage.getItem('track'));
        if (savedTrack && savedTrack.id === trackId) {
            setTrack(savedTrack);
        }
        else {
            setTrack({ name: "name", allowedCustomizations: ["default"] })
            //get track from api
        }

        getFirstTracks();
    }, [trackId]);

    const loadNext = async () => {
        setAddTrackLoading(true);
        const addTracks = await getEventsByTrackId(trackId, takeSize, events.length)
        const extendedTracks = events.concat(addTracks.tracks);
        setHasNext(Array.isArray(addTracks.tracks) && addTracks.tracks.length === takeSize)
        setTracks(extendedTracks);
        setAddTrackLoading(false);
    }


    return (
        <>
            {isStartTrackLoading ? <LinearProgress /> :
                <>
                    <Typography variant="h4" className={classes.title}>
                        {track.name}
                    </Typography>
                    <Typography variant="h4" className={classes.title}>
                        {track.allowedCustomizations.join("  ")}
                    </Typography>
                    <div className={classes.buttonContainer}>
                        <Link to={`/tracks/${track.id}/createEvent`}style={{ textDecoration: 'none' }}>
                            <Button
                                variant="contained"
                                size="large"
                                color="default"
                                startIcon={<AddCircleOutlineIcon />}
                            >
                                Добавить событие
                            </Button>
                        </Link>
                    </div>
                    {Array.isArray(events) && events.length ?
                        <>
                            {events.map(t => <EventBox id={t.id} trackId={trackId} createdAt={t.createdAt} {...t.customizations} />)}
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
                    }
                </>
            }
        </>
    );
}