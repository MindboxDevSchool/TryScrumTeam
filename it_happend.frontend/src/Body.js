import { Container, CssBaseline } from '@material-ui/core';
import { makeStyles } from '@material-ui/core/styles';
import React from 'react';
import Tracks from './Tracks';

const useStyles = makeStyles((theme) => ({
    root: {
        display: 'flex',
        backgroundColor: theme.palette.info.light,
    },
    container: {
    }
}));


export default function Body() {
    const classes = useStyles();

    return (
        <div className={classes.root}>
            <CssBaseline />
            <Container maxWidth="lg" className={classes.container}>
                <Tracks />
            </Container>
        </div>
    );
}
