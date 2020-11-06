import React from 'react';
import { Button, Dialog, DialogTitle, DialogContent, Divider, Typography } from '@material-ui/core';


export default function StatisticsDialog({ buttonText, titleText, statistics, isLoading }) {
    const [isOpen, setOpen] = React.useState(false);

    const onOpen = () => {
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
                onClick={onOpen}>
                {buttonText}
            </Button>
            <Dialog
                open={isOpen}
                onClose={onClose}
            >
                <DialogTitle >{titleText}</DialogTitle>
                <DialogContent>
                    {isLoading ? "Выполняется загрузка..." :
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