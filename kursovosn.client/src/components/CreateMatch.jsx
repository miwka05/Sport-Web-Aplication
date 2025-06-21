// CreateMatch.jsx
import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import axios from 'axios';

const CreateMatch = () => {
    const { id } = useParams(); // tournamentId
    const navigate = useNavigate();
    const [teams, setTeams] = useState([]);
    const [team1, setTeam1] = useState('');
    const [team2, setTeam2] = useState('');
    const [date, setDate] = useState('');
    const [time, setTime] = useState('');
    const [stageName, setStageName] = useState('');
    const [stageOrder, setStageOrder] = useState('');

    useEffect(() => {
        axios.get(`http://localhost:5082/api/tournament/${id}/teams`)
            .then(res => setTeams(res.data.$values))
            .catch(err => console.error(err));
    }, [id]);

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            if (team1 === team2) {
                alert("Выберите разные команды.");
                return;
            }
            await axios.post('http://localhost:5082/api/match/create', {
                tournament_ID: parseInt(id),
                team1_ID: parseInt(team1),
                team2_ID: parseInt(team2),
                data: date,
                time: time,
                stage_Name: stageName,
                stage_Order: parseInt(stageOrder)
            });
            navigate(`/tournaments/${id}`);
        } catch (error) {
            console.error('Ошибка при создании матча:', error);
        }
    };

    return (
        <form onSubmit={handleSubmit}>
            <h2>Создание матча</h2>
            <select value={team1} onChange={e => setTeam1(e.target.value)} required>
                <option value=''>Выберите первую команду</option>
                {teams.map(t => <option key={t.id} value={t.id}>{t.name}</option>)}
            </select>
            <select value={team2} onChange={e => setTeam2(e.target.value)} required>
                <option value=''>Выберите вторую команду</option>
                {teams.map(t => <option key={t.id} value={t.id}>{t.name}</option>)}
            </select>
            <input type='date' value={date} onChange={e => setDate(e.target.value)} required />
            <input type='time' value={time} onChange={e => setTime(e.target.value)} required />
            <input
                type="text"
                placeholder="Название этапа"
                value={stageName}
                onChange={e => setStageName(e.target.value)}
                required
            />
            <input
                type="number"
                placeholder="Порядок этапа"
                value={stageOrder}
                onChange={e => setStageOrder(e.target.value)}
                required
            />
            <button type='submit'>Создать матч</button>
        </form>
    );
};

export default CreateMatch;