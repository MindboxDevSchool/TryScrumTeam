import { makeStyles } from '@material-ui/core/styles';
import React from 'react';
import { useState, useEffect } from "react";
import { getTracks } from './api';
import TrackBox from './TrackBox';

const useStyles = makeStyles((theme) => ({
}));


export default function Tracks() {
    const classes = useStyles();
    const [tracks, setTracks] = useState([]);

    useEffect(() => {
        const fetchPostContent = async () => {
            const { tracks } = await getTracks();
            setTracks(tracks);
        }
        fetchPostContent();
    }, []);


    return (
        <>
            {tracks ?
                tracks.map(t => <TrackBox {...t} />)
                : null
            }
        </>
    );
}
