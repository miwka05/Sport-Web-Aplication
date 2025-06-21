import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { tournamentService } from '../services/tournamentService';

const TournamentList = () => {
    const [tournaments, setTournaments] = useState([]);
    const [bannedTournaments, setBannedTournaments] = useState([]);

    useEffect(() => {
        tournamentService.getAllTournaments()
            .then(data => {
                if (Array.isArray(data)) {
                    const active = data.filter(t => !t.ban);
                    const banned = data.filter(t => t.ban);
                    setTournaments(active);
                    setBannedTournaments(banned);
                } else {
                    console.error("Unexpected data format:", data);
                }
            })
            .catch(error => {
                console.error("Failed to fetch tournaments:", error);
            });
    }, []);

    return (
        <div>
            <h1>Список турниров</h1>
            <ul>
                {tournaments.map(tournament => (
                    <li key={tournament.id} style={{ marginBottom: '1rem' }}>
                        <h2>{tournament.name}</h2>
                        <p><strong>Вид спорта:</strong> {tournament.sportName}</p>
                        <p><strong>Дата:</strong> {new Date(tournament.start).toLocaleDateString()} – {new Date(tournament.end).toLocaleDateString()}</p>
                        <p><strong>Одиночный турнир:</strong> {tournament.solo ? 'Да' : 'Нет'}</p>
                        <Link to={`/tournaments/${tournament.id}`}>Подробнее</Link>
                    </li>
                ))}
            </ul>

            {bannedTournaments.length > 0 && (
                <>
                    <h2>Заблокированные турниры</h2>
                    <ul>
                        {bannedTournaments.map(t => (
                            <li key={t.id} style={{ marginBottom: '1rem', color: 'gray' }}>
                                <h2>{t.name}</h2>
                                <p><strong>Вид спорта:</strong> {t.sportName}</p>
                                <p><strong>Дата:</strong> {new Date(t.start).toLocaleDateString()} – {new Date(t.end).toLocaleDateString()}</p>
                                <p><strong>Причина блокировки:</strong> {t.reason || 'не указана'}</p>
                                <Link to={`/tournaments/${t.id}`}>Подробнее</Link>
                            </li>
                        ))}
                    </ul>
                </>
            )}
        </div>
    );
};

export default TournamentList;

