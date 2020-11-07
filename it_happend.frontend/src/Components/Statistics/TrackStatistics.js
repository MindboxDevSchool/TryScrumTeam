import React from 'react';
import { getTrackStatistics } from '../../api';
import StatisticsDialog from './StatisticsDialog';

export default function TrackStatistics(props) {
    const { id } = props;
    const [isLoading, setLoading] = React.useState(true);
    const [statistics, setStatistics] = React.useState([]);

    React.useEffect(() => {
        const getStatistics = async () => {
            const { statistics } = await getTrackStatistics(id);
            setStatistics(statistics);
            setLoading(false);
        }
        getStatistics();
    }, [id]);

    return (
        <StatisticsDialog
            buttonText="Статистика отслеживания"
            titleText="Статистика отслеживания"
            statistics={statistics}
            isLoading={isLoading} />
    );
}