import React from 'react';
import { Button, Dialog, DialogTitle, DialogContent, Divider, Typography } from '@material-ui/core';
import { getTrackStatistics } from '../../api';


export default function TrackStatistics(props) {
    const { id } = props;
    const [isLoading, setLoading] = React.useState(true);
    const [statistics, setStatistics] = React.useState([]);
    const [isOpen, setOpen] = React.useState(false);

    React.useEffect(() => {
        const getStatistics = async () => {
            const { trackStatistics } = await getTrackStatistics(id);
            setStatistics(trackStatistics);
            setLoading(false);
        }
        getStatistics();
    }, [id]);

    const onClick = () => {
        setOpen(true);
    }

    const onClose = () => {
        setOpen(false);
    }

    return (
        <div>
            <Button
                variant="contained"
                color="default"
                onClick={onClick}>
                Статистика отслеживания
            </Button>
            <Dialog
                open={isOpen}
                onClose={onClose}
                aria-labelledby="alert-dialog-title"
                aria-describedby="alert-dialog-description"
            >
                <DialogTitle id="alert-dialog-title">{"Статистика отслеживания"}</DialogTitle>
                <DialogContent>
                    {isLoading ? "загрузка" :
                        (Array.isArray(statistics) && statistics.length ?
                            <>
                                <Divider />
                                {
                                    statistics.map(fact =>
                                        <>
                                            <Typography>
                                                {fact}
                                            </Typography>
                                            <Divider />
                                        </>)
                                }

                            </>
                            :
                            "Имеющихся событий не достаточно для подсчета стистики. Пользуйтесь приложением и скоро тут появятся данные =)")
                    }
                </DialogContent>
            </Dialog>
        </div>
    );
}