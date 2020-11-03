import { AppBar, Button, Toolbar, Typography, Link } from '@material-ui/core';
import { makeStyles } from '@material-ui/core/styles';

const useStyles = makeStyles((theme) => ({
    '@global': {
        ul: {
            margin: 0,
            padding: 0,
            listStyle: 'none',
        },
    },
    appBar: {
        borderBottom: `1px solid ${theme.palette.divider}`,
    },
    toolbar: {
        flexWrap: 'wrap',
    },
    toolbarTitle: {
        flexGrow: 1,
    },
    link: {
        margin: theme.spacing(1, 1.5),
    }
}));


export default function Header({ onLogout }) {
    const classes = useStyles();

    const username = localStorage.getItem("login");
    
    return (
        <AppBar position="static" color="default" elevation={0} className={classes.appBar}>
            <Toolbar className={classes.toolbar}>
                <Typography variant="h6" color="inherit" noWrap className={classes.toolbarTitle}>
                    TryScrumTeam
                </Typography>
                <Typography variant="body1">
                    Привет, {username}!
                </Typography>
                <Button color="primary" variant="outlined" className={classes.link} onClick={onLogout}>
                    Выйти
                </Button>
            </Toolbar>
        </AppBar>
    )
}