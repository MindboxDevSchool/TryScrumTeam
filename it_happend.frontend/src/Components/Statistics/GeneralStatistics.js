import React from 'react';
import StatisticsDialog from './StatisticsDialog';

export default function GeneralStatistics() {
    const [isLoading, setLoading] = React.useState(true);
    const [statistics, setStatistics] = React.useState([]);

    React.useEffect(() => {
        const getStatistics = async () => {
            // TODO call get statustics
            setStatistics([]);
            setLoading(false);
        }
        getStatistics();
    }, []);

    return (
        <StatisticsDialog
            buttonText="Общая статистика"
            titleText="Статистика всех событий"
            statistics={statistics}
            isLoading={isLoading} />
    );
}