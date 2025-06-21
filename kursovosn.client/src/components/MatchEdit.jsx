// MatchPage.jsx
import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import axios from 'axios';

const MatchEdit = () => {
    const { id } = useParams();
    const [match, setMatch] = useState(null);

    useEffect(() => {
        axios.get(`http://localhost:5082/api/match/${id}`)
            .then(res => setMatch(res.data))
            .catch(err => console.error(err));
    }, [id]);

    if (!match) return <div>Загрузка...</div>;

    return (
        <div>
            <h2>Матч</h2>
            <p>Команды: {match.team1?.name} vs {match.team2?.name}</p>
            <p>Дата: {new Date(match.data).toLocaleDateString()}</p>
            <p>Время: {match.time}</p>
        </div>
    );
};

export default MatchEdit;