import React from 'react';
import { Typography, IconButton, Accordion, AccordionSummary, AccordionDetails } from '@material-ui/core';
import { makeStyles } from '@material-ui/core/styles';
import { deleteTrack } from './api';
import { Edit, Delete, ExpandMore, ExpandLess } from '@material-ui/icons';
import { Link } from "react-router-dom"
import moment from 'moment-timezone'
import 'moment/locale/ru'
moment.locale('ru')

const useStyles = makeStyles((theme) => ({
    trackBox: {
        marginBottom: theme.spacing(2),
    },
    accordion: {
        borderRadius: 10,
        cursor: "default",
    },
    trackSummary: {
        flex: 1,
        display: 'flex',
        flexDirection: 'row',
        alignItems: 'center'
    },
    flex1: {
        flex: '1'
    },
    expandedButton: {
        marginRight: theme.spacing(2)
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

export default function TrackBox(props) {
    const { id, name, createdAt, allowedCustomizations } = props;
    const classes = useStyles();
    const [expanded, setExpanded] = React.useState(false);
    const [isDeleting, setDeleting] = React.useState(false);
    const [isDeleted, setDeleted] = React.useState(false);
    const handleChange = (event) => {
        event.preventDefault();
        setExpanded(!expanded);
    };

    const onDeleteTrack = async (event) => {
        if (window.confirm('Вы точно хотите удалить это отслеживание?')) {
            setDeleting(true);
            event.preventDefault();
            var deletedId = await deleteTrack(id);
            if (deletedId)
                setDeleted(true);
            setDeleting(false);
        }
        event.preventDefault()
    }
    const onRouteToEvents = () => {
        localStorage.setItem('track', JSON.stringify(props))
    }

    return (
        isDeleted ? null :
            <div className={classes.trackBox}>
                <Link to={`/tracks/${id}`} style={{ textDecoration: 'none' }} onClick={onRouteToEvents}>
                    <Accordion square expanded={expanded} className={classes.accordion}>
                        <AccordionSummary aria-controls="id" id="id">
                            <div className={classes.trackSummary}>
                                <IconButton
                                    className={classes.expandedButton}
                                    aria-label="expand"
                                    onClick={handleChange}
                                    onFocus={(event) => event.stopPropagation()}>
                                    {expanded ? <ExpandLess /> : <ExpandMore />}
                                </IconButton>
                                <Typography

                                    variant="subtitle1" className={classes.flex1}>{name} </Typography>
                                <Typography variant="subtitle2" className={classes.flex1}>{moment(createdAt).tz(moment.tz.guess()).format('LL')}</Typography>
                                <Link to={`/editTrack`} onClick={onRouteToEvents}>
                                    <IconButton
                                        aria-label="edit"
                                        //onClick={(event) => event.stopPropagation()}
                                        onFocus={(event) => event.stopPropagation()}>
                                        <Edit fontSize="small" />
                                    </IconButton>
                                </Link>
                                <IconButton
                                    aria-label="delete"
                                    onClick={onDeleteTrack}
                                    onFocus={(event) => event.stopPropagation()}
                                    disabled={isDeleting}>
                                    <Delete fontSize="small" />
                                </IconButton>
                            </div>
                        </AccordionSummary>
                        <AccordionDetails>
                            <Typography>
                                {allowedCustomizations ?
                                    "Дополнительно к отслеживанию можно указать: " + allowedCustomizations.map(customizationsMap).join(", ")
                                    : "У отслеживания не заданы кастомизации"
                                }
                            </Typography>
                        </AccordionDetails>
                    </Accordion>
                </Link>
            </div>
    );
}
