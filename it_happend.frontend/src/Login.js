import React from 'react';
import { useState } from "react";
import { CssBaseline, TextField, AppBar, Tab, Container, Button } from '@material-ui/core';
import { TabContext, TabList, TabPanel } from '@material-ui/lab';
import { makeStyles } from '@material-ui/core/styles';
import { createUser, loginUser } from './api';
import { red } from '@material-ui/core/colors';
 

const useStyles = makeStyles((theme) => ({
    paper: {
        marginTop: theme.spacing(8),
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'center',
    },
    form: {
        width: '100%', // Fix IE 11 issue.
        marginTop: theme.spacing(1),
    },
    errorMessage: {
        color: red[500]
    },
    submit: {
        margin: theme.spacing(3, 0, 2),
    },
}));

function LoginForm({ onSubmit, buttonText, errorMessage, login, setLogin, password, setPassword}) {
    const classes = useStyles();
    const [errorLogin, setErrorLogin] = useState(false);
    const [errorPassword, setErrorPassword] = useState(false);
    const onInput = () => {
        console.log(login);
        if (!login) {
            setErrorLogin(true);
        }
        if (!password) {
            setErrorPassword(true);
        }
        if (errorLogin || errorPassword)
            return;
        onSubmit();
    }
    return (
        <form className={classes.form} noValidate>
            <TextField
                value={login}
                onChange={(e) => setLogin(e.target.value)}
                variant="outlined"
                margin="normal"
                required
                fullWidth
                id="login"
                label="Логин"
                name="login"
                error={errorLogin}
                autoFocus
            />
            <TextField
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                variant="outlined"
                margin="normal"
                required
                fullWidth
                name="password"
                label="Пароль"
                type="password"
                id="password"
                error={errorPassword}
            />
            {errorMessage ?
                <div className={classes.errorMessage}>
                    {errorMessage}
                </div>
                :
                null}
            <Button
                //type="submit"
                fullWidth
                variant="contained"
                color="primary"
                className={classes.submit}
                //onSubmit don't work for material-ui buttons
                onClick={onInput}
            >
                {buttonText}
            </Button>
        </form>
    )
}

export default function Login({ onLogin }) {
    const classes = useStyles();
    const [tabLogin, setTabLogin] = useState("login");
    const [login, setLogin] = useState("");
    const [password, setPassword] = useState("");

    const [errorMessage, setErrorMessage] = useState(null);
    const changeTab = () => {
        if (tabLogin === "login") setTabLogin("registration");
        else
            setTabLogin("login");
        setErrorMessage(null);
    }

    const getUserToken = async() => {
        var token = await loginUser(login, password);
        if (!token) {
            setErrorMessage("Неправильная пара логин-пароль!")
            return;
        }
        onLogin(login, token.accessToken);
    }

    const onLoginButton = async () => {
        setErrorMessage(null);
        getUserToken();
    }
    const onRegistrationButton = async () => {
        setErrorMessage(null);
        console.log("registration");
        var user = await createUser(login, password);
        if (!user) {
            setErrorMessage("Пользователь с таким логином уже существует!")
            return;
        }
        getUserToken();
    }
    return (
        <Container component="main" maxWidth="xs">
            <CssBaseline />
            <div className={classes.paper}>
                <TabContext value={tabLogin}>
                    <AppBar position="static">
                        <TabList onChange={changeTab} aria-label="simple tabs example" centered variant="fullWidth">
                            <Tab label="Вход" value="login" />
                            <Tab label="Регистрация" value="registration" />
                        </TabList>
                    </AppBar>
                    <TabPanel value="login">
                        <LoginForm 
                        onSubmit={onLoginButton} 
                        buttonText="Войти" 
                        errorMessage={errorMessage}
                        login={login}
                        setLogin={setLogin}
                        password={password}
                        setPassword={setPassword}/>
                    </TabPanel>
                    <TabPanel value="registration">
                        <LoginForm 
                        onSubmit={onRegistrationButton} 
                        buttonText="Зарегистрироваться" 
                        errorMessage={errorMessage}
                        login={login}
                        setLogin={setLogin}
                        password={password}
                        setPassword={setPassword}/>
                    </TabPanel>
                </TabContext>
            </div>
        </Container>
    );
}