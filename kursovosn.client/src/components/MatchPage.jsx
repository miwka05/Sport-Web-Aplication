import React, { useEffect, useState } from 'react';
import { useParams, Link } from 'react-router-dom';
import axios from 'axios';

const MatchPage = () => {
    const { id } = useParams();
    const [match, setMatch] = useState(null);
    const [teamStats, setTeamStats] = useState([]);
    const [playerStats, setPlayerStats] = useState([]);
    const [score, setScore] = useState(null);

    useEffect(() => {
        const fetchMatch = async () => {
            try {
                const response = await axios.get(`http://localhost:5082/api/match/${id}`);
                setMatch(response.data);
            } catch (error) {
                console.error("Ошибка загрузки матча:", error);
            }
        };

        const fetchTeamStats = async () => {
            try {
                const response = await axios.get(`http://localhost:5082/api/match/${id}/team-stats`);
                setTeamStats(response.data.$values);
            } catch (error) {
                console.error("Ошибка загрузки статистики команд:", error);
            }
        };

        const fetchPlayerStats = async () => {
            try {
                const response = await axios.get(`http://localhost:5082/api/match/${id}/player-stats`);
                setPlayerStats(response.data.$values);
            } catch (error) {
                console.error("Ошибка загрузки статистики игроков:", error);
            }
        };

        const fetchScore = async () => {
            try {
                const response = await axios.get(`http://localhost:5082/api/match/${id}/score`);
                setScore(response.data);
            } catch (error) {
                console.error("Ошибка загрузки счета:", error);
            }
        };

        fetchMatch();
        fetchTeamStats();
        fetchPlayerStats();
        fetchScore();
    }, [id]);

    if (!match) return <div>Загрузка...</div>;

    return (
        <div>
            <h1>Матч {match.team1?.name} vs {match.team2?.name}</h1>
            <p>Дата: {new Date(match.data).toLocaleDateString()}</p>
            <p>Время: {match.time}</p>
            {score && (
                <p><strong>Счет:</strong> {score.team1Goals} - {score.team2Goals}</p>
            )}
            <Link to={`/matches/${match.id}/Result`}>Запись результатов</Link>

            <h2>Статистика команд</h2>
            <table>
                <thead>
                    <tr>
                        <th>Команда</th>
                        <th>Показатель</th>
                        <th>Значение</th>
                    </tr>
                </thead>
                <tbody>
                    {teamStats.map(team => (
                        <React.Fragment key={team.teamId}>
                            <tr>
                                <td colSpan="3"><strong>{team.teamName}</strong></td>
                            </tr>
                            {(team.stats?.$values || []).map((stat, idx) => (
                                <tr key={idx}>
                                    <td></td>
                                    <td>{stat.statName}</td>
                                    <td>{stat.value}</td>
                                </tr>
                            ))}
                        </React.Fragment>
                    ))}
                </tbody>
            </table>

            <h2>Статистика игроков</h2>
            <table>
                <thead>
                    <tr>
                        <th>Игрок</th>
                        <th>Показатель</th>
                        <th>Значение</th>
                    </tr>
                </thead>
                <tbody>
                    {playerStats.map((stat, idx) => (
                        <tr key={idx}>
                            <td>{stat.playerName}</td>
                            <td>{stat.statName}</td>
                            <td>{stat.value}</td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
};

export default MatchPage;
