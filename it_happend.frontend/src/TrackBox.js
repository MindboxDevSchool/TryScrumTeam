import React from 'react';
import { Typography, IconButton, Accordion, AccordionSummary, AccordionDetails } from '@material-ui/core';
import { makeStyles } from '@material-ui/core/styles';
import { Edit, Delete } from '@material-ui/icons';
import moment from 'moment'
import 'moment/locale/ru'
moment.locale('ru')

const useStyles = makeStyles((theme) => ({
    trackBox: {
        marginBottom: theme.spacing(2),
    },
    accordion: {
        borderRadius: 10
    },
    trackSummary: {
        flex: 1,
        display: 'flex',
        flexDirection: 'row',
        alignItems: 'center'
    },
    flex1: {
        flex: '1'
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

export default function TrackBox({ id, name, createdAt, allowedCustomizations }) {
    const classes = useStyles();
    const [expanded, setExpanded] = React.useState(false);

    const handleChange = () => (event, newExpanded) => {
        setExpanded(newExpanded ? true : false);
    };

    return (
        <div className={classes.trackBox}>
            <Accordion square expanded={expanded} onChange={handleChange()} className={classes.accordion}>
                <AccordionSummary aria-controls="id" id="id">
                    <div className={classes.trackSummary}>
                        <Typography variant="subtitle1" className={classes.flex1}>{name} </Typography>
                        <Typography variant="subtitle2" className={classes.flex1}>{moment(createdAt).format('LL')}</Typography>
                        <IconButton
                            aria-label="edit"
                            onClick={(event) => event.stopPropagation()}
                            onFocus={(event) => event.stopPropagation()}>
                            <Edit fontSize="small" />
                        </IconButton>
                        <IconButton
                            aria-label="delete"
                            onClick={(event) => event.stopPropagation()}
                            onFocus={(event) => event.stopPropagation()}>
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
        </div>
    );
}
