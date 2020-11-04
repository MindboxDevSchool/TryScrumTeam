import React from "react";
import { makeStyles } from "@material-ui/core/styles";
import Card from "@material-ui/core/Card";
import CardActions from "@material-ui/core/CardActions";
import CardContent from "@material-ui/core/CardContent";
import Button from "@material-ui/core/Button";
import Typography from "@material-ui/core/Typography";
import CardMedia from '@material-ui/core/CardMedia';


import Rating from '@material-ui/lab/Rating';

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
                    Comment
                    </Typography>
                    <Typography variant="h5" component="h2">
                    {props.commentText}
                    </Typography>
                    </div>;
    
    const rating = <div>
        <Typography
          className={classes.title}
          color="textSecondary"
          gutterBottom
        >
          Rating
        </Typography>
        <Rating max={10}  value={props.rating} readOnly/>
        </div>;

    const geoTag = <div>
        <Typography
          className={classes.title}
          color="textSecondary"
          gutterBottom
        >
          GeoTag
        </Typography>
        <Typography className ={classes.geoTag} display="inline" variant="h5" component="h2">
        Attitude  {props.geoTag[0]}  
        </Typography>
        <Typography display="inline" variant="h5" component="h2">
        Longitude {props.geoTag[1]}  
        </Typography>
    </div>;

    const scale = <div>
        <Typography
          className={classes.title}
          color="textSecondary"
          gutterBottom
        >
          Scale
        </Typography>
        <Typography variant="h5" component="h2">
          {props.scale}
        </Typography>
    </div>;

    const photo = <div>
        <Typography
          className={classes.title}
          color="textSecondary"
          gutterBottom
        >
          Photo
        </Typography>
        <img src={props.photoUrl}/>
    </div>;
    
    

  return (
      
    <Card className={classes.root}>
      <CardContent >
      <Typography
          className={classes.title}
          color="textSecondary"
          gutterBottom
        >
          {props.date.toUTCString()}
        </Typography>
        {props.commentText != null
        ?
        comment
        :
        <div/>
        }
        {props.rating != null
        ?
        rating
        :
        <div/>
        }
        {props.scale != null
        ?
        scale
        :
        <div/>
        }
        {props.geoTag != null
        ?
        geoTag
        :
        <div/>
        }
        {props.photoUrl != null
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
