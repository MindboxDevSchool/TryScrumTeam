import React from "react";
import { makeStyles } from "@material-ui/core/styles";
import { Card, CardActions, CardContent, Typography, Divider, IconButton } from "@material-ui/core";
import { Edit, Delete, Comment, Room } from '@material-ui/icons';
import Rating from '@material-ui/lab/Rating';
import { Link } from "react-router-dom"
import { deleteEvent } from './../../api';
import { grey } from '@material-ui/core/colors';

import moment from 'moment'
import 'moment/locale/ru'
moment.locale('ru')

const useStyles = makeStyles((theme) => ({
  root: {
    width: 390,
    marginBottom: theme.spacing(2),
    marginRight: theme.spacing(2),
    borderRadius: 10
  },
  bullet: {
    display: "inline-block",
    margin: "0 2px",
    transform: "scale(0.8)"
  },
  title: {
    fontSize: 14,
    marginRight: theme.spacing(1)
  },
  pos: {
    marginBottom: 12
  },
  geoTag: {
    marginRight: 10
  },
  cardActions: {
    display: 'flex',
    justifyContent: 'flex-end'
  },
  ratingContainer: {
    marginTop: theme.spacing(2),
    marginBottom: theme.spacing(2),
    display: 'flex',
    alignItems: 'center'
  },
  photoContainer: {
    display: 'flex',
    justifyContent: 'center',
    height: 200,
  },

  geotagContainer: {
    marginTop: theme.spacing(1),
    marginBottom: theme.spacing(1),
    display: 'flex',
    alignItems: 'center'
  },
  commentContainer: {
    backgroundColor: grey[200],
    padding: theme.spacing(2),
    borderRadius: 10,
    position: 'relative',
    marginTop: theme.spacing(2),
    marginBottom: theme.spacing(2),
  }

}));
export default function EventBox(props) {
  const classes = useStyles();
  const comment =
    <div className={classes.commentContainer}>
      <Comment />
      <Typography variant="h6" component="h2">
        {props.comment}
      </Typography>
    </div>;

  const rating =
    <div className={classes.ratingContainer}>
      <Typography className={classes.title} color="textSecondary">
        Оценка:
      </Typography>
      <Rating max={10} value={props.rating} readOnly />
    </div>;

  function openGoogleMapInNewTab() {
    const url = `https://www.google.com/maps/place/${props.geotagLatitude}+${props.geotagLongitude}`;
    var win = window.open(url, '_blank');
    win.focus();
  }

  const geoTag =
    <div className={classes.geotagContainer}>
      <Typography
        className={classes.title}
        color="textSecondary"
      >
        Местоположение:
        </Typography>
      <IconButton
        onClick={openGoogleMapInNewTab}>
        <Room />
      </IconButton>
    </div>;

  const scale =
    <div className={classes.ratingContainer}>
      <Typography
        className={classes.title}
        color="textSecondary"
      >
        Шкала:
      </Typography>
      <Typography variant="h5" component="h2">
        {props.scale}
      </Typography>
    </div>;

  const photo =
    <div className={classes.photoContainer}>
      <img src={props.photoUrl} alt="" width="auto" height="100%" />
    </div>;

  const onRouteToEdit = () => {
    localStorage.setItem('event', JSON.stringify(props))
  }


  const [isDeleting, setDeleting] = React.useState(false);
  const [isDeleted, setDeleted] = React.useState(false);

  const DeleteEvent = async (event) => {
    if (window.confirm('Вы точно хотите удалить это событие?')) {
      setDeleting(true);
      event.preventDefault();
      var deletedId = await deleteEvent(props.trackId, props.id);
      if (deletedId)
        setDeleted(true);
      setDeleting(false);
    }
    event.preventDefault()
  }

  return (
    <div>
      {!isDeleted
        ?
        <Card className={classes.root}>
          <CardContent >
            <Typography
              variant='h6'
              color="textPrimary"
              gutterBottom
            >
              {moment(props.createdAt).format('LL h:mm a')}
            </Typography>
            <Divider />
            {props.rating != null ? rating : null}
            {props.comment != null ? comment : null}
            {props.scale != null ? scale : null}
            {props.geotagLatitude != null && props.geotagLongitude != null ? geoTag : null}
            {props.photoUrl != null ? photo : null}

            {props.rating != null || props.comment != null || props.scale != null || props.geotagLatitude != null || props.geotagLongitude != null || props.photoUrl != null
              ? <Divider />
              : null
            }
          </CardContent>
          <CardActions className={classes.cardActions}>
            <Link to={`/tracks/${props.trackId}/events/${props.id}/edit`} onClick={onRouteToEdit}>
              <IconButton aria-label="edit">
                <Edit fontSize="small" />
              </IconButton>
            </Link>
            <IconButton
              aria-label="delete"
              onClick={DeleteEvent}
              disabled={isDeleting}>
              <Delete fontSize="small" />
            </IconButton>
          </CardActions>
        </Card>
        :
        <div />
      }
    </div>
  );
}
