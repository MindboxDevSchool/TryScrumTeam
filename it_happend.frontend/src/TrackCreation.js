import React from 'react';
import { makeStyles } from '@material-ui/core/styles';
import Typography from '@material-ui/core/Typography';
import TextField from '@material-ui/core/TextField';
import Checkbox from '@material-ui/core/Checkbox';
import FormLabel from '@material-ui/core/FormLabel';
import FormControl from '@material-ui/core/FormControl';
import FormGroup from '@material-ui/core/FormGroup';
import FormControlLabel from '@material-ui/core/FormControlLabel';
import FormHelperText from '@material-ui/core/FormHelperText';
import Button from '@material-ui/core/Button';
import { createTrack,editTrack } from './api';
import history from './history' 

export default function TrackCreation({isEdit = false}) {
  if(isEdit)
  {
    var track = JSON.parse(localStorage['track'])
  }
  const useStyles = makeStyles((theme) => ({
    root: {
      display: 'flex',
    },
    formControl: {
      margin: theme.spacing(3),
    },
    container: {
      backgroundColor: '#ffffff',
      padding: '20px',
      borderBottomLeftRadius: '10px',
      borderBottomRightRadius: '10px',
      marginLeft: '25vw',
      marginRight: '25vw',
      display: 'flex',
      alignContent: 'center',
      flexDirection: 'column',
    }
  }));

  const classes = useStyles();

  const [customs, setState] = React.useState({
    Comment: isEdit ? track.allowedCustomizations.includes("Comment"):false,
    Rating: isEdit ? track.allowedCustomizations.includes("Rating"):false,
    Scale: isEdit ? track.allowedCustomizations.includes("Scale"):false,
    Photo: isEdit ? track.allowedCustomizations.includes("Photo"):false,
    Geotag: isEdit ? track.allowedCustomizations.includes("Geotag"):false
  });

  const [text, setText] = React.useState(isEdit ? track.name :"")
  const handleTextChange = (event) => {
    setText(event.target.value);
    setButtonDisabled(false)
  };

  var wrongText = false
  if (text === "")
    wrongText = true;

  const [IsButtonDisabled, setButtonDisabled] = React.useState((isEdit ? true : false));

  const handleCustomsSelectorChange = (event) => {
    setState({ ...customs, [event.target.name]: event.target.checked });
    setButtonDisabled(false)
  };

  const { Comment, Rating, Scale, Photo, Geotag } = customs;

  const Submit = async (event) => {
    var listOfCustoms = Object.keys(customs)
      .filter(function (k) { return customs[k] })
      .map(String)

    const trackInfo = {
      "name": text,
      "CreatedAt": new Date(),
      "allowedCustomizations": listOfCustoms
    }
    setButtonDisabled(true);
    if(isEdit)
      await editTrack(trackInfo,track.id);
    else
      await createTrack(trackInfo);
    setButtonDisabled(false);
    history.push('/')
  };

  return (
    <div className={classes.container}>
      <Typography variant="h6" gutterBottom>
        {isEdit ? "Изменение отслеживания": "Новое отслеживание"}
      </Typography>
      <TextField
        onChange={handleTextChange}
        required
        label="Название"
        defaultValue ={isEdit ? track.name :""} 
      />
      {wrongText
        ?
        <FormHelperText>Название не может быть пустым</FormHelperText>
        :
        <div />
      }
      <FormControl required component="fieldset" className={classes.formControl}>
        <FormLabel component="legend">Кастомизации</FormLabel>
        <FormGroup>
          <FormControlLabel
            control={<Checkbox color="primary" checked={Comment} onChange={handleCustomsSelectorChange} name="Comment" />}
            label="Комментарий"
          />
          <FormControlLabel
            control={<Checkbox color="primary" checked={Rating} onChange={handleCustomsSelectorChange} name="Rating" />}
            label="Оценка"
          />
          <FormControlLabel
            control={<Checkbox color="primary" checked={Scale} onChange={handleCustomsSelectorChange} name="Scale" />}
            label="Шкала"
          />
          <FormControlLabel
            control={<Checkbox color="primary" checked={Photo} onChange={handleCustomsSelectorChange} name="Photo" />}
            label="Фото"
          />
          <FormControlLabel
            control={<Checkbox color="primary" checked={Geotag} onChange={handleCustomsSelectorChange} name="Geotag" />}
            label="Местоположение"
          />
        </FormGroup>
      </FormControl>
      <Button disabled={IsButtonDisabled || wrongText} variant="contained" color="primary" onClick={Submit}>
      {isEdit ? "Изменить": "Создать"}
      </Button>
    </div>
  );
}