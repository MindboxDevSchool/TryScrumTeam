import React from 'react';
import { makeStyles } from '@material-ui/core/styles';
import { useState, useEffect } from "react";
import Grid from '@material-ui/core/Grid';
import Typography from '@material-ui/core/Typography';
import TextField from '@material-ui/core/TextField';
//import FormControlLabel from '@material-ui/core/FormControlLabel';
import Checkbox from '@material-ui/core/Checkbox';
import { Multiselect } from 'multiselect-react-dropdown';
import FormLabel from '@material-ui/core/FormLabel';
import FormControl from '@material-ui/core/FormControl';
import FormGroup from '@material-ui/core/FormGroup';
import FormControlLabel from '@material-ui/core/FormControlLabel';
import FormHelperText from '@material-ui/core/FormHelperText';
import Button from '@material-ui/core/Button';
import {createTrack} from './api';
//import Checkbox from '@material-ui/core/Checkbox';
 





export default function TrackCreation() {

    const useStyles = makeStyles((theme) => ({
        root: {
            display: 'flex',
          },
          formControl: {
            margin: theme.spacing(3),
          },
        container: {
            
            marginLeft: '25vw',
            marginRight: '25vw'
        }
    }));
    
    const [cutoms, setState] = React.useState({
        Comment:true,
        Rating:false,
        Scale:false,
        Photo:false,
        Geotag:false
      });

    const [text,setText] = React.useState("")
    const handleTextChange = (event) => {
        setText(event.target.value);
      };


      const handleChange = (event) => {
        setState({ ...cutoms, [event.target.name]: event.target.checked });
      };
    
      const { Comment, Rating, Scale ,Photo, Geotag} = cutoms;
      const error = [Comment, Rating, Scale ,Photo, Geotag].filter((v) => v).length == 0;
      var wrongText = false
      if(text == "")
      wrongText = true;

      var disableButton = wrongText || error;

    const classes = useStyles();
    
    

    const Submit = (event) => {
        var listOfCustoms =Object.keys(cutoms)
        .filter(function(k){return cutoms[k]})
        .map(String)
        
        const trackInfo = {"name": text,
        "CreatedAt": new Date(),
        "allowedCustomizations":listOfCustoms}
        console.log(trackInfo);
        createTrack(trackInfo);
      };

  return (
    
    <React.Fragment>
    
      <div className = {classes.container}>
      <Typography variant="h6" gutterBottom>
        Название отслеживания
      </Typography>
          <TextField
          onChange = {handleTextChange}
            required
            label="Название"
            
          />
          {wrongText
        ?
        <FormHelperText>Название не может быть пустым</FormHelperText>
        :
        <div/>
        }

    <FormControl required error={error} component="fieldset" className={classes.formControl}>
        <FormLabel component="legend">Кастомизации</FormLabel>
        <FormGroup>
          <FormControlLabel
            control={<Checkbox color="primary" checked={Comment} onChange={handleChange} name="Comment" />}
            label="Комментарий"
          />
          <FormControlLabel
            control={<Checkbox color="primary" checked={Rating} onChange={handleChange} name="Rating" />}
            label="Оценка"
          />
          <FormControlLabel
            control={<Checkbox color="primary" checked={Scale} onChange={handleChange} name="Scale" />}
            label="Шкала"
          />
          <FormControlLabel
            control={<Checkbox color="primary" checked={Photo} onChange={handleChange} name="Photo" />}
            label="Фото"
          />
          <FormControlLabel
            control={<Checkbox color="primary" checked={Geotag} onChange={handleChange} name="Geotag" />}
            label="ГеоТег"
          />
        </FormGroup>
        {error
        ?
        <FormHelperText>Выберите хотя бы одну</FormHelperText>
        :
        <div/>
        }
      </FormControl>
      
      <Button disabled = {disableButton} variant="contained" color="primary" onClick = {Submit}>
        Создать
      </Button>
        </div>
        
    
    </React.Fragment>
    
  );
}