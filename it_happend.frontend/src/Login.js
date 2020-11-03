import React from 'react';
import { useState } from "react";
import { CssBaseline, TextField, AppBar, Tab, Container, Button, CircularProgress } from '@material-ui/core';
import { TabContext, TabList, TabPanel } from '@material-ui/lab';
import { makeStyles } from '@material-ui/core/styles';
import { createUser, loginUser } from './api';
import { red, grey } from '@material-ui/core/colors';


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
    wrapperButton: {
        position: 'relative',
    },
    buttonProgress: {
        color: grey[500],
        position: 'absolute',
        top: '50%',
        left: '50%',
        marginTop: -7,
        marginLeft: -12,
    },
}));

function isLoginValid(value) {
    return !!value;
}

function isPasswordValid(value) {
    return !!value;
}

function LoginForm({ onSubmit, buttonText, errorMessage, login, setLogin, password, setPassword, isDisabled }) {
    const classes = useStyles();
    const [errorLogin, setErrorLogin] = useState(false);
    const [errorPassword, setErrorPassword] = useState(false);
    const onInput = () => {
        setErrorLogin(!isLoginValid(login));
        setErrorPassword(!isPasswordValid(password));
        if (isLoginValid(login) && isPasswordValid(password))
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
            <div className={classes.wrapperButton}>
                <Button
                    fullWidth
                    variant="contained"
                    color="primary"
                    className={classes.submit}
                    onClick={onInput}
                    disabled={isDisabled}
                >
                    {buttonText}
                </Button>
                {isDisabled ? <CircularProgress size={24} className={classes.buttonProgress} /> : null}
            </div>
        </form>
    )
}

export default function Login({ onLogin }) {
    const classes = useStyles();

    const [tabLogin, setTabLogin] = useState("login");
    const [isDisabled, setDisabled] = useState(false);
    const [login, setLogin] = useState("");
    const [password, setPassword] = useState("");
    const [errorMessage, setErrorMessage] = useState(null);

    const changeTab = () => {
        if (tabLogin === "login") setTabLogin("registration");
        else
            setTabLogin("login");
        setErrorMessage(null);
    }

    const getUserToken = async () => {
        var token = await loginUser(login, password);
        if (!token) {
            setErrorMessage("Неправильная пара логин-пароль!")
            return;
        }
        onLogin(login, token.accessToken);
    }

    const onLoginButton = async () => {
        setDisabled(true);
        setErrorMessage(null);
        await getUserToken();
        setDisabled(false);
    }

    const onRegistrationButton = async () => {
        setDisabled(true);
        setErrorMessage(null);
        var user = await createUser(login, password);
        if (!user) {
            setErrorMessage("Пользователь с таким логином уже существует!")
            return;
        }
        await getUserToken();
        setDisabled(false);
    }

    var loginFormParams = {};
    loginFormParams.errorMessage = errorMessage;
    loginFormParams.login = login;
    loginFormParams.setLogin = setLogin;
    loginFormParams.password = password;
    loginFormParams.setPassword = setPassword;
    loginFormParams.isDisabled = isDisabled;

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
                            {...loginFormParams} />
                    </TabPanel>
                    <TabPanel value="registration">
                        <LoginForm
                            onSubmit={onRegistrationButton}
                            buttonText="Зарегистрироваться"
                            {...loginFormParams} />
                    </TabPanel>
                </TabContext>
            </div>
        </Container>
    );
}