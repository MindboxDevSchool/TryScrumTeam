import { makeStyles } from '@material-ui/core/styles';
import React from 'react';
import { useState, useEffect } from "react";
import { getEventsByTrackId } from './../../api';
import EventBox from './EventBox';
import { Button, Typography, LinearProgress } from '@material-ui/core';
import AddCircleOutlineIcon from '@material-ui/icons/AddCircleOutline';
import { useParams } from "react-router-dom";
import { Link } from "react-router-dom"
import TrackStatistics from "../Statistics/TrackStatistics"
import { useHistory } from 'react-router-dom'

const useStyles = makeStyles((theme) => ({
    title: {
        marginTop: theme.spacing(5),
    },
    subTitle: {
        marginTop: theme.spacing(3),
        marginBottom: theme.spacing(3),
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
    },
    eventContainer: {
        display: 'flex',
        //alignContent: 'space-between',
        //justifyContent: 'space-between',
        flexWrap: 'wrap'
    }
}));

const customizationsMap = ((value) => {
    switch (value) {
        case 'Comment':
            return "комментарий"
        case 'Rating':
            return "рейтинг"
        case 'Scale':
            return "шкала"
        case 'Photo':
            return "фотография"
        case 'Geotag':
            return "местоположение"
        default:
            return ""
    }
});

export default function Events() {
    const { trackId } = useParams();
    const classes = useStyles();
    const takeSize = 10;

    //track fields { id, name, createdAt, allowedCustomizations } 
    const [track, setTrack] = useState({});
    const [events, setEvents] = useState([]);
    const [hasNext, setHasNext] = useState(false);
    const [isAddEventLoading, setAddEventLoading] = useState(false);
    const [isStartEventLoading, setStartEventLoading] = useState(true);

    useEffect(() => {
        const getFirstEvents = async () => {
            const { events } = await getEventsByTrackId(trackId, takeSize);
            setHasNext(Array.isArray(events) && events.length === takeSize)
            setEvents(events);
            setStartEventLoading(false);
        }

        var savedTrack = JSON.parse(localStorage.getItem('track'));
        if (savedTrack && savedTrack.id === trackId) {
            setTrack(savedTrack);
        }
        else {
            setTrack({ name: "name", allowedCustomizations: ["default"] })
            //get track from api
        }

        getFirstEvents();
    }, [trackId]);

    const loadNext = async () => {
        setAddEventLoading(true);
        const addEvents = await getEventsByTrackId(trackId, takeSize, events.length)
        const extendedEvents = events.concat(addEvents.events);
        setHasNext(Array.isArray(addEvents.events) && addEvents.events.length === takeSize)
        setEvents(extendedEvents);
        setAddEventLoading(false);
    }


    const [locationKeys, setLocationKeys] = useState([])
    const history = useHistory()

    useEffect(() => {
        return history.listen(location => {
            if (history.action === 'PUSH') {
                setLocationKeys([location.key])
            }
            if (history.action === 'POP') {
                if (locationKeys[1] === location.key) {
                    setLocationKeys(([_, ...keys]) => keys)
                    // Handle forward event
                } else {
                    setLocationKeys((keys) => [location.key, ...keys])
                    history.push('/')
                }
            }
        })
    }, [locationKeys,])


    return (
        <>
            {isStartEventLoading ? <LinearProgress /> :
                <>
                    <Typography variant="h4" className={classes.title}>
                        {track.name}
                    </Typography>
                    <Typography variant="h5" className={classes.subTitle}>
                        {'Для добавления доступны : ' + track.allowedCustomizations.map(customizationsMap).join(", ")}
                    </Typography>
                    <TrackStatistics id={track.id} />
                    <div className={classes.buttonContainer}>
                        <Link to={`/tracks/${track.id}/createEvent`} style={{ textDecoration: 'none' }}>
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
                            <div className={classes.eventContainer}>
                                {events.map(t => <EventBox id={t.id} trackId={trackId} createdAt={t.createdAt} {...t.customizations} />)}
                            </div>
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
                            Ничего еще не случилось. Самое время добавить новое событие.
                    </Typography>
                    }
                </>
            }
        </>
    );
}