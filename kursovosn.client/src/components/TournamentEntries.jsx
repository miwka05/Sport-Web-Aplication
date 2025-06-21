import React, { useEffect, useState, useContext } from 'react';
import { useParams, useNavigate, Link } from 'react-router-dom';
import axios from 'axios';
import { UserContext } from './UserContext'; // Контекст авторизации


const TournamentEntries = () => {
    const { id } = useParams(); // ID турнира
    const [entries, setEntries] = useState([]);
    const [tournament, setTournament] = useState(null);
    const currentUser = useContext(UserContext);
    //const navigate = useNavigate();

    useEffect(() => {
        fetchEntries();
        fetchTournament();
    }, []);

    const fetchEntries = async () => {
        try {
            const token = sessionStorage.getItem("token");
            const response = await axios.get(`http://localhost:5082/api/tournament/${id}/entries`, {
                headers: { Authorization: `Bearer ${token}` }
            });
            setEntries(response.data.$values || []);
        } catch (error) {
            console.error('Ошибка загрузки заявок:', error);
        }
    };


    const fetchTournament = async () => {
        try {
            const response = await axios.get(`http://localhost:5082/api/tournament/${id}`);
            setTournament(response.data);
        } catch (error) {
            console.error('Ошибка загрузки турнира:', error);
        }
    };

    const handleDecision = async (entryId, decision) => {
        try {
            const token = sessionStorage.getItem("token");
            await axios.post(`http://localhost:5082/api/tournament/entry/${entryId}/${decision}`, {}, {
                headers: { Authorization: `Bearer ${token}` }
            });
            fetchEntries();
        } catch (error) {
            console.error('Ошибка при обработке заявки:', error);
        }
    };

    if (!tournament || !currentUser) return <div>Загрузка...</div>;

    if (tournament.creator_ID !== currentUser.id) {
        return <div>Доступ запрещён. Вы не являетесь организатором турнира.</div>;
    }

    return (
        <div>
            <h2>Заявки на турнир "{tournament.name}"</h2>
            {entries.length === 0 ? (
                <p>Заявок пока нет.</p>
            ) : (
                <ul>
                    {entries.map((entry) => (
                        <li key={entry.id}>
                            {entry.userName && (
                                <p>
                                    Пользователь: <Link to={`/profile/${entry.userId}`}>{entry.userName}</Link>
                                </p>
                            )}
                            {entry.teamName && (
                                <p>
                                    Команда: <Link to={`/team/${entry.teamId}`}>{entry.teamName}</Link>
                                </p>
                            )}
                            <p>Доп. информация: {entry.info}</p>
                            <button onClick={() => handleDecision(entry.id, 'accept')}>Принять</button>
                            <button onClick={() => handleDecision(entry.id, 'reject')}>Отклонить</button>
                        </li>
                    ))}
                </ul>
            )}
        </div>
    );
};

export default TournamentEntries;


