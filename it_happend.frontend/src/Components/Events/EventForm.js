import React from 'react';
import { useState } from "react";
import { IconButton, TextField, Divider, Button, List, ListItem, ListItemText, CircularProgress, Typography, Tooltip } from '@material-ui/core';
import { makeStyles } from '@material-ui/core/styles';
import { red, grey } from '@material-ui/core/colors';
import { Add, Clear, HelpOutlineOutlined } from '@material-ui/icons';
import Rating from '@material-ui/lab/Rating';
import { getAddressSuggestions, getGeotag, getBingGeotag } from '../../mapApi';
import AsyncSelect from 'react-select/async';

const useStyles = makeStyles((theme) => ({
    container: {
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'center',
    },
    root: {
        marginTop: theme.spacing(5),
        width: '70vh',
        backgroundColor: theme.palette.background.paper,
        borderRadius: "10px"

    },
    errorMessage: {
        color: red[500]
    },
    margin16: {
        marginRight: theme.spacing(2)
    },
    marginVertical: {
        marginBottom: theme.spacing(1),
        marginTop: theme.spacing(1)
    },
    wrapperButton: {
        position: 'relative',
    },
    buttonProgress: {
        color: grey[500],
        position: 'absolute',
        top: '50%',
        left: '50%',
        marginTop: -12,
        marginLeft: -12,
    },
    tooltipContainer: {
        display: 'flex',
        alignItems: 'center',
    },
}));

const AddCustomIcon = ({ error, isChoosen, onChange }) => {
    return (
        <IconButton
            edge="start"
            onClick={(e) => onChange('isChoosen', !isChoosen)}>
            {isChoosen ? <Clear style={error ? { color: red[500] } : null} /> : <Add />}
        </IconButton>
    )
}

function CommentCustom({ value, isChoosen, error, onChange }) {
    return (
        <ListItem id="comment">
            <AddCustomIcon isChoosen={isChoosen} error={error} onChange={onChange} />
            {isChoosen ?
                <TextField
                    value={value}
                    onChange={(e) => onChange('value', e.target.value)}
                    variant="outlined"
                    margin="normal"
                    fullWidth
                    label="Комментарий"
                    error={error}
                    autoFocus
                />
                :
                <ListItemText>Добавьте комментарий</ListItemText>}
        </ListItem>
    );
}

function RatingCustom({ value, error, isChoosen, onChange }) {
    return (
        <ListItem id="rating">
            <AddCustomIcon isChoosen={isChoosen} error={error} onChange={onChange} />
            {isChoosen ?
                <Rating
                    value={value}
                    max={10}
                    name="rating"
                    onChange={(event, newValue) =>
                        onChange('value', newValue)}
                />
                :
                <ListItemText>Поставьте оценку</ListItemText>}
        </ListItem>
    );
}

function isFloat(n) {
    return !isNaN(parseFloat(n.match(/^-?\d*(\.\d*)?$/)));
}

function ScaleCustom({ value, error, isChoosen, onChange }) {
    const onInputChange = (e) => {
        if (isFloat(e.target.value) || e.target.value === "") {
            onChange('value', e.target.value);
        }
    }
    return (
        <ListItem id="scale">
            <AddCustomIcon isChoosen={isChoosen} error={error} onChange={onChange} />
            {isChoosen ?
                <TextField
                    value={value}
                    onChange={onInputChange}
                    variant="outlined"
                    margin="normal"
                    label="Шкала"
                    name="scale"
                    error={error}
                    autoFocus
                />
                :
                <ListItemText>Введите данные шкалы</ListItemText>}
        </ListItem>
    );
}

function GeotagCustom({ value, error, isChoosen, onChange }) {
    const classes = useStyles();

    const onLatitudeChange = (e) => {
        if (isFloat(e.target.value) || e.target.value === "") {
            onChange('value', { latitude: e.target.value, longitude: value.longitude });
        }
    }
    const onLongitudeChange = (e) => {
        if (isFloat(e.target.value) || e.target.value === "") {
            onChange('value', { latitude: value.latitude, longitude: e.target.value });
        }
    }

    const [selectedValue, setSelectedValue] = useState(null);

    const setGeotagForAddress = async (value) => {
        const res = await getBingGeotag(value);
        if (res.resourceSets[0].estimatedTotal !== 0) {
            const data = res.resourceSets[0].resources[0].point.coordinates;
            onChange('value', { latitude: String(data[0]), longitude: String(data[1]) });
        }
    }

    const handleChange = value => {
        setSelectedValue(value);
        if (value.value) {
            setGeotagForAddress(value.value);
        }
    }

    const loadOptions = (inputValue) => {
        return getAddressSuggestions(inputValue).then(res => res.suggestions);
    };

    return (
        <ListItem id="geotag">
            <AddCustomIcon isChoosen={isChoosen} error={error} onChange={onChange} />
            {isChoosen ?
                <div style={{ width: '100%' }}>
                    <div className={classes.tooltipContainer}>
                        <Typography className={classes.marginVertical}> Выполните поиск по адресу: </Typography>
                        <Tooltip title="Если при выборе адреса координаты не заполнились, попробуйте ввести более(менее) точный адрес">
                            <IconButton size="small">
                                <HelpOutlineOutlined />
                            </IconButton>
                        </Tooltip>
                    </div>
                    <AsyncSelect
                        styles={{ container: (base) => ({ ...base, zIndex: 500 }) }}
                        noOptionsMessage={() => "Не могу найти адрес"}
                        placeholder={"Введите адрес..."}
                        cacheOptions
                        defaultOptions
                        value={selectedValue}
                        getOptionLabel={e => e.value}
                        getOptionValue={e => e.value}
                        loadOptions={loadOptions}
                        onChange={handleChange}
                    />
                    <Typography className={classes.marginVertical}> Или задайте координаты вручную </Typography>
                    <div style={{ display: 'flex' }}>
                        <TextField
                            value={value.latitude}
                            onChange={onLatitudeChange}
                            variant="outlined"
                            margin="normal"
                            label="Широта"
                            error={error}
                            autoFocus
                        />
                        <div className={classes.margin16} />
                        <TextField
                            value={value.longitude}
                            onChange={onLongitudeChange}
                            variant="outlined"
                            margin="normal"
                            label="Долгота"
                            error={error}
                        />
                    </div>
                </ div >
                :
                <ListItemText>Укажите местоположение</ListItemText>}
        </ListItem>
    );
}

function PhotoCustom({ value, isChoosen, error, onChange }) {
    return (
        <ListItem id="photo">
            <AddCustomIcon isChoosen={isChoosen} error={error} onChange={onChange} />
            {isChoosen ?
                <TextField
                    value={value}
                    onChange={(e) => onChange('value', e.target.value)}
                    variant="outlined"
                    margin="normal"
                    fullWidth
                    label="Url к фото"
                    error={error}
                    autoFocus
                />
                :
                <ListItemText>Загрузите фотографию</ListItemText>}
        </ListItem>
    );
}

export default function EventForm({ allowedCustomizations, event, onSave, isEdit }) {
    const classes = useStyles();
    const isNotNumber = (value) => {
        return value === undefined || value === null;
    }
    const [comment, setComment] = useState(
        {
            value: isEdit ? event.comment : "",
            isChoosen: isEdit && event.comment,
            error: null
        }
    );
    const [rating, setRating] = useState(
        {
            value: isEdit ? event.rating : null,
            error: null,
            isChoosen: isEdit && event.rating,
        }
    );
    const [scale, setScale] = useState(
        {
            value: isEdit ? String(event.scale) : "",
            error: null,
            isChoosen: isEdit && !isNotNumber(event.scale),
        }
    );
    const [geotag, setGeotag] = useState(
        {
            value: {
                latitude: isEdit ? String(event.geotagLatitude) : "",
                longitude: isEdit ? String(event.geotagLatitude) : ""
            },
            error: null,
            isChoosen: isEdit && !isNotNumber(event.geotagLatitude),
        }
    );

    const [photo, setPhoto] = useState(
        {
            value: isEdit ? event.photoUrl : "",
            error: null,
            isChoosen: isEdit && event.photoUrl,
        }
    );
    const [isDisabled, setDisabled] = useState(false);

    const onChange = (property, setter, field, value) => {
        var newValue = property;
        newValue[field] = value;
        setter({ ...newValue });
    }

    const onSaveButton = () => {
        setDisabled(true);
        var errors = false;
        var listOfCustoms = {};
        if (comment.isChoosen) {
            var error = !comment.value;
            errors = errors || error;
            onChange(comment, setComment, 'error', error);
            listOfCustoms["Comment"] = comment.value;
        }
        if (rating.isChoosen) {
            error = !rating.value;
            errors = errors || error;
            onChange(rating, setRating, 'error', error);
            listOfCustoms["Rating"] = rating.value;
        }
        if (scale.isChoosen) {
            error = !scale.value;
            errors = errors || error;
            onChange(scale, setScale, 'error', error);
            listOfCustoms["Scale"] = parseFloat(scale.value);
        }
        if (photo.isChoosen) {
            error = !photo.value;
            errors = errors || error;
            onChange(photo, setPhoto, 'error', error);
            listOfCustoms["PhotoUrl"] = photo.value;
        }
        if (geotag.isChoosen) {
            error = !geotag.value.latitude || !geotag.value.longitude;
            errors = errors || error;
            onChange(geotag, setGeotag, 'error', error);
            listOfCustoms["GeotagLatitude"] = parseFloat(geotag.value.latitude);
            listOfCustoms["GeotagLongitude"] = parseFloat(geotag.value.longitude);
        }
        var date = new Date()
        date.setMinutes(date.getMinutes() - (new Date()).getTimezoneOffset());
        const eventContent = {
            "CreatedAt": isEdit ? event.CreatedAt : date,
            "customizations": listOfCustoms
        }
        if (!errors) {
            onSave(eventContent);
        }
        setDisabled(false);
    }


    return (
        <div className={classes.container}>
            <div className={classes.root}>
                {Array.isArray(allowedCustomizations) && allowedCustomizations.length ?
                    <List >
                        {allowedCustomizations.includes("Comment") ?
                            <>
                                <CommentCustom onChange={(f, v) => onChange(comment, setComment, f, v)} {...comment} />
                                <Divider />
                            </>
                            : null}
                        {allowedCustomizations.includes("Rating") ?
                            <>
                                <RatingCustom onChange={(f, v) => onChange(rating, setRating, f, v)} {...rating} />
                                <Divider />
                            </>
                            : null}
                        {allowedCustomizations.includes("Scale") ?
                            <>
                                <ScaleCustom onChange={(f, v) => onChange(scale, setScale, f, v)} {...scale} />
                                <Divider />
                            </>
                            : null}
                        {allowedCustomizations.includes("Geotag") ?
                            <>
                                <GeotagCustom onChange={(f, v) => onChange(geotag, setGeotag, f, v)} {...geotag} />
                                <Divider />
                            </>
                            : null}
                        {allowedCustomizations.includes("Photo") ?
                            <>
                                <PhotoCustom onChange={(f, v) => onChange(photo, setPhoto, f, v)} {...photo} />
                                <Divider />
                            </>
                            : null}
                    </List>
                    : null
                }
                <div className={classes.wrapperButton}>
                    <Button
                        fullWidth
                        variant="contained"
                        color="primary"
                        size="large"
                        className={classes.submit}
                        onClick={onSaveButton}
                        disabled={isDisabled}
                    >
                        {isEdit ? "Сохранить событие" : "Добавить событие"}
                    </Button>
                    {isDisabled ? <CircularProgress size={24} className={classes.buttonProgress} /> : null}
                </div>
            </div>
        </div>
    );
}