import { Container, CssBaseline } from '@material-ui/core';
import { makeStyles } from '@material-ui/core/styles';
import React from 'react';
import Tracks from './Tracks';
import EventList from './Components/Events/EventList';
import { BrowserRouter, Switch, Route } from "react-router-dom"
import TrackCreation from "./TrackCreation"


const useStyles = makeStyles((theme) => ({
    root: {
        display: 'flex',
        backgroundColor: theme.palette.info.light,
        minHeight: '100vh'
    },
    container: {
        paddingBottom: theme.spacing(3)
    }
}));

export default function Body() {
    const classes = useStyles();

    return (
        <div className={classes.root}>
            <CssBaseline />
            <Container maxWidth="lg" className={classes.container}>
                <BrowserRouter>
                    <Switch>
                        <Route path="/tracks/:trackId" component={EventList} />
                        <Route path="/newTrack" component={TrackCreation} />
                        <Route path="/editTrack" >
                            <TrackCreation isEdit = {true} />
                        </Route>
                        <Route path="/" component={Tracks} />
                    </Switch>
                </BrowserRouter>
            </Container>
        //</div>
    );
}
