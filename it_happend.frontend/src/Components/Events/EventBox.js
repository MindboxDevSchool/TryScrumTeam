import React from "react";
import { makeStyles } from "@material-ui/core/styles";
import {Card,CardActions,CardContent,Typography,Button} from "@material-ui/core";
import moment from 'moment'
import 'moment/locale/ru'
import Rating from '@material-ui/lab/Rating';


moment.locale('ru')

const useStyles = makeStyles({
  root: {
    minWidth: 275
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
  geoTag:{
      marginRight: 10
  }
});
//var props = {commentText : "fsfsf"}
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
                    {props.Comment}
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
        <Rating max={10}  value={props.Rating} readOnly/>
        </div>;

    const geoTag = <div>
        <Typography
          className={classes.title}
          color="textSecondary"
          gutterBottom
        >
          ГеоТег
        </Typography>
        <Typography className ={classes.geoTag} display="inline" variant="h5" component="h2">
        Широта  {props.GeotagLatitude}  
        </Typography>
        <Typography display="inline" variant="h5" component="h2">
        Долгота {props.GeotagLongitude}  
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
          {props.Scale}
        </Typography>
    </div>;

    const photo = <div>
        <img src={props.PhotoUrl}/>
    </div>;
    
    

  return (
      
    <Card className={classes.root}>
      <CardContent >
      <Typography
          className={classes.title}
          color="textSecondary"
          gutterBottom
        >
          {moment(props.CreatedAt).format('LL h:mm a')}
        </Typography>
        {props.Comment != null
        ?
        comment
        :
        <div/>
        }
        {props.Rating != null
        ?
        rating
        :
        <div/>
        }
        {props.Scale != null
        ?
        scale
        :
        <div/>
        }
        {props.GeotagLatitude != null && props.GeotagLongitude != null
        ?
        geoTag
        :
        <div/>
        }
        {props.PhotoUrl != null
        ?
        photo
        :
        <div/>
        }
      </CardContent>
      <CardActions>
        <Button size="small">Delete</Button>
        <Button size="small">Edit</Button>
      </CardActions>
    </Card>
  );
}
