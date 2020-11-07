import { makeStyles } from '@material-ui/core/styles';
import React from 'react';
import { Button, Typography } from '@material-ui/core';
import { Link } from "react-router-dom"

const useStyles = makeStyles((theme) => ({
    paper: {
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'center',
    },
    title: {
        marginTop: theme.spacing(10),
        marginBottom: theme.spacing(10)
    },
}));


export default function NotFound() {
    const classes = useStyles();

    return (
        <>
            <div className={classes.paper}>
                <Typography variant="h5" className={classes.title}>
                    Страница не существует.
                </Typography>
                <Link to={`/`} style={{ textDecoration: 'none' }}>
                    <Button
                        variant="contained"
                        size="large"
                        color="default"
                    >
                        Перейти на главную
                    </Button>
                </Link>
            </div>
        </>
    );
}
