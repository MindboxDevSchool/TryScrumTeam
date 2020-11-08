import React from "react";
import { makeStyles } from "@material-ui/core/styles";
import { Card, CardActions, CardContent, Typography, Button } from "@material-ui/core";
import Rating from '@material-ui/lab/Rating';
import { Link } from "react-router-dom"
import { deleteEvent } from './../../api';
import moment from 'moment-timezone' 
import 'moment/locale/ru'
moment.locale('ru')

const useStyles = makeStyles((theme) => ({
  root: {
    minWidth: 275,
    marginBottom: theme.spacing(2),
    borderRadius: 10
  },
  bullet: {
    display: "inline-block",
    margin: "0 2px",
    transform: "scale(0.8)"
  },
  title: {
    fontSize: 14
  },
  pos: {
    marginBottom: 12
  },
  geoTag: {
    marginRight: 10
  }
}));
export default function EventBox(props) {
  const classes = useStyles();
  const comment = <div>
    <Typography
      className={classes.title}
      color="textSecondary"
      gutterBottom
    >
      Комментарий
                    </Typography>
    <Typography variant="h5" component="h2">
      {props.comment}
    </Typography>
  </div>;

  const rating = <div>
    <Typography
      className={classes.title}
      color="textSecondary"
      gutterBottom
    >
      Оценка
        </Typography>
    <Rating max={10} value={props.rating} readOnly />
  </div>;

  const geoTag = <div>
    <Typography
      className={classes.title}
      color="textSecondary"
      gutterBottom
    >
      Местоположение
        </Typography>
    <Typography className={classes.geoTag} display="inline" variant="h5" component="h2">
      Широта  {props.geotagLatitude}
    </Typography>
    <Typography display="inline" variant="h5" component="h2">
      Долгота {props.geotagLongitude}
    </Typography>
  </div>;

  const scale = <div>
    <Typography
      className={classes.title}
      color="textSecondary"
      gutterBottom
    >
      Шкала
        </Typography>
    <Typography variant="h5" component="h2">
      {props.scale}
    </Typography>
  </div>;

  const photo = <div>
    <img src={props.photoUrl} width="50%" />
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
              className={classes.title}
              color="textSecondary"
              gutterBottom
            >
              {moment(props.createdAt).tz(moment.tz.guess()).format('LL h:mm a')}
            </Typography>
            {props.comment != null
              ?
              comment
              :
              <div />
            }
            {props.rating != null
              ?
              rating
              :
              <div />
            }
            {props.scale != null
              ?
              scale
              :
              <div />
            }
            {props.geotagLatitude != null && props.geotagLongitude != null
              ?
              geoTag
              :
              <div />
            }
            {props.photoUrl != null
              ?
              photo
              :
              <div />
            }
          </CardContent>
          <CardActions>
            <Button disable={isDeleting} size="small" onClick={DeleteEvent}>Удалить</Button>
            <Link to={`/tracks/${props.trackId}/events/${props.id}/edit`} style={{ textDecoration: 'none' }} onClick={onRouteToEdit}>
              <Button size="small">Изменить</Button>
            </Link>
          </CardActions>
        </Card>
        :
        <div />
      }
    </div>
  );
}
