import { makeStyles } from '@material-ui/core/styles';
import React from 'react';
import { useState, useEffect } from "react";
import { getTracks } from './api';
import TrackBox from './TrackBox';
import { Button, Typography, LinearProgress } from '@material-ui/core';
import AddCircleOutlineIcon from '@material-ui/icons/AddCircleOutline';
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
        justifyContent: 'flex-end',
        marginTop: theme.spacing(2),
        marginBottom: theme.spacing(4),
    },
    loadButtonContainer: {
        display: 'flex',
        justifyContent: 'center',
    }
}));


export default function Tracks() {
    const classes = useStyles();
    const takeSize = 10;
    const [tracks, setTracks] = useState([]);
    const [hasNext, setHasNext] = useState(false);
    const [isAddTrackLoading, setAddTrackLoading] = useState(false);
    const [isStartTrackLoading, setStartTrackLoading] = useState(true);

    useEffect(() => {
        const getFirstTracks = async () => {
            const { tracks } = await getTracks(takeSize);
            setHasNext(Array.isArray(tracks) && tracks.length === takeSize)
            setTracks(tracks);
            setStartTrackLoading(false);
        }
        getFirstTracks();
    }, []);

    const loadNext = async () => {
        setAddTrackLoading(true);
        const addTracks = await getTracks(takeSize, tracks.length)
        const extendedTracks = tracks.concat(addTracks.tracks);
        setHasNext(Array.isArray(addTracks.tracks) && addTracks.tracks.length === takeSize)
        setTracks(extendedTracks);
        setAddTrackLoading(false);
    }

    return (
        <>
            <Typography variant="h4" className={classes.title}>
                Отслеживания
            </Typography>
            <div className={classes.buttonContainer}>
            <Link to={`/newTrack/`} style={{ textDecoration: 'none' }}>
                <Button
                    variant="contained"
                    size="large"
                    color="default"
                    startIcon={<AddCircleOutlineIcon />}
                >
                    Добавить отслеживание
                </Button>
            </Link>
            </div>
            {isStartTrackLoading ? <LinearProgress /> :
                (Array.isArray(tracks) && tracks.length ?
                    <>
                        {tracks.map(t => <TrackBox {...t} />)}
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
                        У тебя пока нет отслеживаний. Попробуй добавить новое
                 </Typography>
                )}
        </>
    );
}
